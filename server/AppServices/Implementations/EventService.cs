using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using EventManager.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace EventManager.AppServices.Implementations;

public class EventService : IEventService
{
    private readonly EventManagerDbContext _context;
    private readonly ILogger<EventService> _logger;

    public EventService(ILogger<EventService> logger, EventManagerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<CreateEventResponse> SaveEventAsync(CreateEventRequest req, Guid ownerId)
    {
        CreateEventResponse res = new();



        // 1) Валидация преди try/catch
        if (req?.Event == null)
        {
            res.StatusCode = BusinessStatusCodeEnum.BadRequest;
            res.Message = "Missing event data.";
            return res;
        }

        if (string.IsNullOrWhiteSpace(req.Event.Title))
        {
            res.StatusCode = BusinessStatusCodeEnum.BadRequest;
            res.Message = "Name is required.";
            return res;
        }

        if (req.Event.StartDate < DateTime.UtcNow.Date)
        {
            res.StatusCode = BusinessStatusCodeEnum.BadRequest;
            res.Message = "Началната дата не може да бъде по-ранна от днес.";
            return res;
        }



        try
        {
            var ev = new Event
            {
                Title = req.Event.Title,
                Location = req.Event.Location,
                Description = req.Event.Description,
                StartDate = req.Event.StartDate, 
                OwnerUserId = ownerId,
                Public = req.Event.Public,
                eventStatus = req.Event.Status,
            };

            await _context.Events.AddAsync(ev);

            // owner auto-joins
            await _context.EventParticipants.AddAsync(
                new EventParticipant { EventId = ev.Id, InviterId = ownerId, InviteeId = ownerId, inviteStatus = InviteStatus.Accepted });

            await _context.SaveChangesAsync();


            res.StatusCode = BusinessStatusCodeEnum.Success;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "CreateEvent DbUpdateException for owner {OwnerId}", ownerId);
            res.StatusCode = BusinessStatusCodeEnum.InternalServerError;
            res.Message = ex.Message;
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating event for owner {OwnerId}", ownerId);
            res.StatusCode = BusinessStatusCodeEnum.InternalServerError;
            res.Message = ex.Message;
            return res;
        }

        return res;
    }

    //edit participents
    public async Task<EditEventResponse> UpdateEventAsync(EditEventRequest req, Guid actingUserId)
    {
        var res = new EditEventResponse();

        var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == req.Event.Id);



        // Съществува ли такъв евент
        if (ev == null)
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.NotFound;
            res.Message = "Събитието не е намерено.";
            return res;
        }

        // Проверка дали съм Организатора
        if (ev.OwnerUserId != actingUserId)
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.Unauthorized;
            res.Message = "Нямате права за редакция.";
            return res;
        }

        
        // Проверка за по стара дата от днес

        if (req.Event.StartDate < DateTime.UtcNow.Date)
        {
            res.StatusCode = BusinessStatusCodeEnum.BadRequest;
            res.Message = "Началната дата не може да бъде по-ранна от днес.";
            return res;
        }
        

        
        ev.Title = req.Event.Title;
        ev.Location = req.Event.Location;
        ev.Description = req.Event.Description;
        ev.StartDate = req.Event.StartDate;
        ev.Public = req.Event.Public;
        ev.eventStatus = req.Event.Status;
          


        await _context.SaveChangesAsync();

        res.StatusCode = Messaging.BusinessStatusCodeEnum.Success;
        res.Message = "Събитието е обновено.";
        return res;
    }



    public async Task<AddParticipantsResponse> SaveParticipantsAsync(AddParticipantsRequest req, Guid actingUserId)
    {
        var res = new AddParticipantsResponse();



        if (req.EventId == Guid.Empty || req.UserIds is null || req.UserIds.Count == 0)
        {
            res.StatusCode = BusinessStatusCodeEnum.BadRequest;
            res.Message = "Невалидна заявка: EventId/UserIds.";
            return res;
        }

        try
        {
            // 1) събитието съществува ли?
            var ev = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == req.EventId);

            if (ev is null)
            {
                res.StatusCode = BusinessStatusCodeEnum.NotFound;
                res.Message = "Събитието не е намерено.";
                return res;
            }

            if (ev.eventStatus != EventStatus.Active)
            {
                res.StatusCode = BusinessStatusCodeEnum.BadRequest;
                res.Message = "Събитието е неактивно/архивирано.";
                return res;
            }

            var isOwner = ev.OwnerUserId == actingUserId;

            var isParticipant = _context.EventParticipants
            .Any(p => p.EventId == ev.Id && p.InviteeId == actingUserId  && p.inviteStatus == InviteStatus.Accepted);


            var hasAccess = isOwner || isParticipant;


            // 2) права (ако само owner може да добавя)
            if (!hasAccess)
            {
                res.StatusCode = BusinessStatusCodeEnum.Unauthorized;
                res.Message = "Само създателят на събитието може да добавя участници.";
                return res;
            }

            // нормализиране на входа
            var incoming = req.UserIds
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (incoming.Count == 0)
            {
                res.StatusCode = BusinessStatusCodeEnum.BadRequest;
                res.Message = "Списъкът с потребители е празен.";
                return res;
            }

            // 3) кои от тези потребители съществуват реално?
            var existingUsers = await _context.Users
                .AsNoTracking()
                .Where(u => incoming.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            var missingUsers = incoming.Except(existingUsers).ToList();

            // 4) кои вече са участници и да ги прескочим
            var existingParticipantIds = await _context.EventParticipants
                .AsNoTracking()
                .Where(ep => ep.EventId == req.EventId && incoming.Contains(ep.InviteeId))
                .Select(ep => ep.InviteeId)
                .ToListAsync();

            var toInsert = existingUsers.Except(existingParticipantIds).ToList();

            // 5) подготвяме записи за добавяне
            var newRows = toInsert.Select(uid => new EventParticipant
            {
                EventId = req.EventId,
                InviterId = actingUserId,
                InviteeId = uid,
                inviteStatus = InviteStatus.Invited
            }).ToList();

            if (newRows.Count > 0)
            {
                await _context.EventParticipants.AddRangeAsync(newRows);
                await _context.SaveChangesAsync();
            }

            res.Added = newRows.Count;
            res.SkippedExisting = existingParticipantIds.Count;
            res.SkippedMissingUsers = missingUsers.Count;
            res.StatusCode = BusinessStatusCodeEnum.Success;
            res.Message = "Участниците са обработени.";
            return res;
        }
        catch (DbUpdateException)
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.InternalServerError;
            res.Message = "Грешка при запис в базата.";
            return res;
        }
        catch
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.InternalServerError;
            res.Message = "Непредвидена грешка.";
            return res;
        }
    }



    public async Task<GetEventsResponse> LoadEventsAsync(GetEventsRequest req)
    {
        var q = _context.Events
            .AsNoTracking()
            .AsQueryable();

        //„всички“
        // напр. публични, и в които участвам (Accepted) ако са Private
        q = q.Where(e =>
            e.Public == true 
            || e.Participants.Any(p => p.InviteeId == req.ActingUserId));
        



        q = q.OrderByDescending(e => e.StartDate);

        var total = await q.CountAsync();


        var items = await q
             .Skip((req.Page - 1) * req.PageSize)
             .Take(req.PageSize)
             .Select(e => new EventViewModel
             {
                 Id = e.Id,
                 Title = e.Title,
                 Location = e.Location,
                 Description = e.Description,
                 StartDate = e.StartDate,
                 Public = e.Public,
                 Status = e.eventStatus,

                 // само owner, ако ти трябва
                 Owner = new UserViewModel
                 {
                     FirstName = e.OwnerUser.FirstName,
                     LastName = e.OwnerUser.LastName,
                     Email = e.OwnerUser.Email
                 },
                 ParticipantCount = e.Participants.Count(p => p.inviteStatus == InviteStatus.Accepted),

             })
             .ToListAsync();

        return new GetEventsResponse
        {
            Events = items,
            Total = total,
            Page = req.Page,
            PageSize = req.PageSize,
            Message = total == 0 ? "Нямате събития." : "Страница: "+ req.Page
        };
    }

    public async Task<GetEventsResponse> LoadMyOwnedEventsAsync(GetEventsRequest req)
    {
        var q = _context.Events
            .AsNoTracking()
            .AsQueryable();

        // Мойте събития

        q = q.Where(e => e.OwnerUserId == req.ActingUserId);

        q = q.OrderByDescending(e => e.StartDate);

        var total = await q.CountAsync();


        var items = await q
             .Skip((req.Page - 1) * req.PageSize)
             .Take(req.PageSize)
             .Select(e => new EventViewModel
             {
                 Id = e.Id,
                 Title = e.Title,
                 Location = e.Location,
                 Description = e.Description,
                 StartDate = e.StartDate,
                 Public = e.Public,
                 Status = e.eventStatus,

                 // само owner, ако ти трябва
                 Owner = new UserViewModel
                 {
                     FirstName = e.OwnerUser.FirstName,
                     LastName = e.OwnerUser.LastName,
                     Email = e.OwnerUser.Email
                 },
                 ParticipantCount = e.Participants.Count(p => p.inviteStatus == InviteStatus.Accepted),

             })
             .ToListAsync();

        return new GetEventsResponse
        {
            Events = items,
            Total = total,
            Page = req.Page,
            PageSize = req.PageSize,
            Message = total == 0 ? "Нямате събития." : "Страница: " + req.Page
        };
    }

    public async Task<GetEventsResponse> LoadMyEventsPraticipateAsync(GetEventsRequest req)
    {
        var q = _context.Events
            .AsNoTracking()
            .AsQueryable();


            // само евенти, където аз участвам и съм ПРИЕЛ
            q = q.Where(e => e.Participants
                .Any(p => p.InviteeId == req.ActingUserId
                       && p.inviteStatus == InviteStatus.Accepted));


        q = q.OrderByDescending(e => e.StartDate);

        var total = await q.CountAsync();


        var items = await q
             .Skip((req.Page - 1) * req.PageSize)
             .Take(req.PageSize)
             .Select(e => new EventViewModel
             {
                 Id = e.Id,
                 Title = e.Title,
                 Location = e.Location,
                 Description = e.Description,
                 StartDate = e.StartDate,
                 Public = e.Public,
                 Status = e.eventStatus,

                 // само owner, ако ти трябва
                 Owner = new UserViewModel
                 {
                     FirstName = e.OwnerUser.FirstName,
                     LastName = e.OwnerUser.LastName,
                     Email = e.OwnerUser.Email
                 },
                 ParticipantCount = e.Participants.Count(p => p.inviteStatus == InviteStatus.Accepted),

             })
             .ToListAsync();

        return new GetEventsResponse
        {
            Events = items,
            Total = total,
            Page = req.Page,
            PageSize = req.PageSize,
            Message = total == 0 ? "Нямате събития." : "Страница: " + req.Page
        };
    }


    public async Task<GetEventByIdResponse> LoadEventByIdAsync(GetEventByIdRequest req, Guid actingUserId, bool isAdmin)
    {

       
        var response = new GetEventByIdResponse();

        try
        {
            var _event = await _context.Events
               .AsNoTracking()
               .Where(e => e.Id == req.EventId)
               .Select(e => new EventViewModel
               {
                   Id = e.Id,
                   Title = e.Title,
                   Location = e.Location,
                   Description = e.Description,
                   StartDate = e.StartDate,
                   Public = e.Public,
                   Status = e.eventStatus,
                   Owner = new UserViewModel
                   {
                       UserId = e.OwnerUserId,
                       FirstName = e.OwnerUser.FirstName,
                       LastName = e.OwnerUser.LastName,
                       Email = e.OwnerUser.Email
                   },
                   Participants = e.Participants
                    .Where(p => p.Invitee != null)
                    .Select(p => new UserViewModel
                    {
                        UserId = p.InviteeId,
                        FirstName = p.Invitee.FirstName,
                        LastName = p.Invitee.LastName,
                        Email = p.Invitee.Email
                    })
                    .ToList()

               })
                   .FirstOrDefaultAsync();

            if (_event == null)
            {
                response.StatusCode = BusinessStatusCodeEnum.NotFound;
                return response;
            }


            // по подразбиране: нямаш достъп
            var hasAccess =
                isAdmin                                   // 1) ако си админ
                || _event.Owner.UserId == actingUserId    // 2) ако си собственик
                || _event.Public                          // 3) ако е публичен
                || _event.Participants.Any(p => p.UserId == actingUserId); // 4) ако участваш


            if (!hasAccess)
            {
                response.StatusCode = BusinessStatusCodeEnum.Unauthorized;
                response.Message = "Нямате достъп до това събитие.";
                return response;
            }


            response.Event = _event;
            response.StatusCode = BusinessStatusCodeEnum.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching event {EventId}", req.EventId);
            response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
        }

        return response;
    }





}
