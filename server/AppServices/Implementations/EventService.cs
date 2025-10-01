using System.Reflection.Metadata.Ecma335;
using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using EventManager.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManager.AppServices.Implementations;

/// <summary>
/// Сервизен клас, който съдържа бизнес логиката за управление на събития.
/// Отговаря за създаване, редакция, покани и статистики.
/// </summary>
public class EventService : IEventService
{
    private readonly EventManagerDbContext _db;
    private readonly ILogger<EventService> _logger;

    /// <summary>
    /// Конструктор, който инициализира <see cref="EventService"/> 
    /// и инжектира необходимите зависимости.
    /// </summary>
    public EventService(EventManagerDbContext db, ILogger<EventService> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Създава ново събитие в базата данни.
    /// </summary>
    /// <param name="req">Модел със заявка за създаване на събитие.</param>
    /// <param name="userId">Идентификатор на потребителя-собственик.</param>
    /// <returns>Отговор със статус за резултата от създаването.</returns>
    public async Task<CreateEventResponse> CreateEvent([FromBody] CreateEventRequest req, string userId)
    {
        var res = new CreateEventResponse();

        if (!Guid.TryParse(userId, out var ownerId))
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.BadRequest;
            return res;
        }

        try
        {
            var ev = new Event
            {
                Title = req.Name,
                Location = req.Location,
                Description = req.Description,
                StartDate = req.StartDate,
                OwnerUserId = ownerId,
                Status = EventStatus.Active
            };

            await _db.Events.AddAsync(ev);
            // owner auto-joins
            await _db.EventParticipants.AddAsync(new EventParticipant { EventId = ev.Id, InviteeId = ownerId, InviterId = ownerId, Status = InviteStatus.Accepted });

            await _db.SaveChangesAsync();

            return res;
        }
        catch (DbUpdateException ex)
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.InternalServerError;

            _logger.LogError(ex, "CreateEvent DbUpdateException for owner {OwnerId}", ownerId);
            res.StatusCode = Messaging.BusinessStatusCodeEnum.BadRequest;

            return res;
        }
    }

    /// <summary>
    /// Връща данни за конкретно събитие по неговото Id.
    /// </summary>
    /// <param name="req">Заявка, съдържаща идентификатора на събитието.</param>
    /// <returns><see cref="EventViewModel"/> с основните данни за събитието и участниците.</returns>
    public async Task<EventViewModel> GetEvent(GetEventRequest req)
    {
        var ev = await _db.Events
            .AsNoTracking()
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == req.EventId);

        if (ev?.OwnerUser?.UserName is not null && ev?.OwnerUser?.Email is not null)
        {
            var participantIds = ev.Participants.ToList();
            var owner = new UserViewModel
            {
                Id = ev.OwnerUserId,
                UserName = ev.OwnerUser.UserName,
                Email = ev.OwnerUser.Email,
            };

            return new EventViewModel
            {
                Id = ev.Id,
                Title = ev.Title!,
                Description = ev.Description,
                StartDate = ev.StartDate,
                Location = ev.Location,
                OwnerUserId = ev.OwnerUserId,
                Participants = ev.Participants,
                Status = ev.Status
            };
        }
        else
        {
            throw new KeyNotFoundException("Event not found");
        }
    }

    /// <summary>
    /// Извлича събитие от базата данни по неговия идентификатор.
    /// </summary>
    /// <param name="id">Уникален идентификатор (<see cref="Guid"/>) на събитието, което трябва да бъде намерено.</param>
    /// <returns>Обект от тип <see cref="GetByIdResponse"/></returns>
    public async Task<GetByIdResponse> GetEventById(Guid id)
    {
        var res = new GetByIdResponse();

        var ev = await _db.Events
            .AsNoTracking()
            .Include(e => e.Participants)
            .ThenInclude(p => p.Invitee)
            .Include(e => e.OwnerUser)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.NotFound;
            return res;
        }

        var participants = ev.Participants
            .Where(p => p.Status == InviteStatus.Accepted)
            .ToList();
        var owner = new UserViewModel
        {
            UserName = ev.OwnerUser!.UserName,
            Email = ev.OwnerUser.Email,
        };

        res.Event = new EventViewModel
        {
            Id = ev.Id,
            Title = ev.Title!,
            Description = ev.Description,
            StartDate = ev.StartDate,
            Location = ev.Location,
            OwnerUserId = ev.OwnerUserId,
            Participants = participants,
            Status = ev.Status
        };

        return res;
     }

    /// <summary>
    /// Редактира съществуващо събитие.
    /// </summary>
    /// <param name="eventId">Идентификатор на събитието за редакция.</param>
    /// <param name="userId">Идентификатор на потребителя, който прави редакция.</param>
    /// <param name="req">Заявка с новите данни за събитието.</param>
    /// <returns>Отговор със статус за успешна редакция.</returns>
    public async Task<EditEventResponse> EditEvent(Guid eventId, Guid userId, EditEventRequest req)
    {
        Console.WriteLine($"Editing event {eventId} by user {userId}");
        var ev = await _db.Events.FirstOrDefaultAsync(e => e.Id == eventId);

        if (ev == null) throw new KeyNotFoundException("Event not found");

        if (ev.OwnerUserId != userId) throw new UnauthorizedAccessException("Only owner can edit");

        Console.WriteLine($"Before Edit: Title={ev.Title}, Location={ev.Location}, Description={ev.Description}, StartDate={ev.StartDate}");
        Console.WriteLine($"Edit Request: Title={req.Title}, Location={req.Location}, Description={req.Description}, StartDate={req.StartDate}");
        if (req.Title is not null) ev.Title = req.Title;
        if (req.Location is not null) ev.Location = req.Location;
        if (req.Description is not null) ev.Description = req.Description;
        // the state of the properties below will be checked and ensured to have values in the FE
        ev.StartDate = req.StartDate;

        await _db.SaveChangesAsync();
        return new EditEventResponse { Success = true };
    }

    /// <summary>
    /// Cancels an event by changing its status to "Cancelled".
    /// </summary>
    /// <param name="eventId">
    /// The Id of the event to be canceled.
    /// </param> 
    /// <param name="userId">
    /// The Id of the user who is trying to cancel the event.
    /// </param>
    /// <returns>The response contains the status of the request and a message to track the process.</returns>
    /// <response code="200">Returns the requested project board.</response>
    /// <response code="404">If the project board is not found.</response>
    /// <response code="403">If the user is not the owner of the event.</response>
    /// <response code="400">If the event is already canceled.</response>   
    public async Task<CancelEventResponse> CancelEvent(Guid eventId, Guid userId)
    {
        var res = new CancelEventResponse();

        var ev = await _db.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (ev == null)
        {
            res.StatusCode = BusinessStatusCodeEnum.NotFound;
            return res;
        }

        if (ev.OwnerUserId != userId)
        {
            res.StatusCode = BusinessStatusCodeEnum.Forbidden;
            return res;
        }

        if (ev.Status == EventStatus.Cancelled)
        {
            res.StatusCode = BusinessStatusCodeEnum.BadRequest;
            res.Message = "Event is already canceled.";
            return res;
        }

        ev.Status = EventStatus.Cancelled;
        await _db.SaveChangesAsync();

        res.StatusCode = BusinessStatusCodeEnum.Success;
        res.Message = "Event canceled successfully.";
        return res;
    }

    /// <summary>
    /// Добавя участници към дадено събитие.
    /// </summary>
    /// <param name="req">Заявка със списък от Id-та на потребители за покана.</param>
    /// <param name="inviterId">Идентификатор на потребителя, който изпраща поканите.</param>
    /// <returns>Отговор със стойност колко участници са били добавени.</returns>
    public async Task<AddParticipantsResponse> AddParticipants(AddParticipantsRequest req, string inviterId)
    {
        var ev = await _db.Events.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == req.EventId);
        if (ev == null) throw new KeyNotFoundException("Event not found");

        // we've accepted that every user can inivite participants to an event.
        //if (ev.OwnerUserId != req.ActorUserId) throw new UnauthorizedAccessException("Only owner can add participants");

        var existing = ev.Participants.Select(p => p.InviteeId).ToHashSet();
        int added = 0;
        foreach (var uid in req.UserIds)
        {
            if (!existing.Contains(uid))
            {
                await _db.EventParticipants.AddAsync(new EventParticipant
                {
                    EventId = ev.Id,
                    InviteeId = uid,
                    InviterId = Guid.Parse(inviterId),
                    Status = Data.Enums.InviteStatus.Invited // added status "Invited" upon invitation as
                                                             // requested in the assignment
                });
                added++;
            }
        }
        await _db.SaveChangesAsync();
        return new AddParticipantsResponse { Added = added };
    }

    /// <summary>
    /// Връща списък с всички събития от системата.
    /// </summary>
    /// <returns><see cref="GetAllEventsResponse"/> с колекция от събития.</returns>                                                      
    public async Task<GetAllEventsResponse> GetAllEvents()
    {
        var response = new GetAllEventsResponse();

        var q = await _db.Events.AsNoTracking().Where(x => x.Status == EventStatus.Active).ToListAsync();

        var allEventsList = q.Select(e => new EventSummary
        {
            Id = e.Id,
            Title = e.Title!,
            Description = e.Description,
            StartDate = e.StartDate,
            OwnerUserId = e.OwnerUserId,
            Status = e.Status,
            Location = e.Location,
            Participants = e.Participants
        })
            .OrderBy(x => x.StartDate)
            .ToList();

        response.Events = allEventsList;

        return response;
    }


    /// <summary>
    /// Генерира статистика за събития на конкретен потребител,
    /// включително проценти на приети/отказани/висящи покани.
    /// </summary>
    /// <param name="ownerId">Идентификатор на собственика на събитията.</param>
    /// <returns><see cref="StatisticViewModel"/> със статистическите данни.</returns>
    public async Task<StatisticViewModel> GetEventStatistic(Guid ownerId)
    {
        var ownerEvents = await _db.Events
            .AsNoTracking()
            .Where(x => x.OwnerUserId == ownerId)
            .Include(x => x.Participants)
            .ToListAsync();

        if (!ownerEvents.Any())
        {
            return new StatisticViewModel
            {
                EventStatistics = new List<EventStatistic>(),
                OwnerEventsCount = 0,
                OwnerId = ownerId
            };
        }

        var res = new StatisticViewModel();
        // Total count of events created by the uuser.
        res.OwnerEventsCount = ownerEvents.Count();

        foreach (var @event in ownerEvents)
        {
            // Calculate percentage of "Accepted" invites against all "Invited".
            var currentEventStatistic = 0.0;
            var eventParticipants = @event.Participants.ToList();

            var participantsWithAcceptedInvites = eventParticipants
                .Where(x => x.Status == InviteStatus.Accepted)
                .Count();

            var participantsWithDeclinedInvites = eventParticipants
                .Where(x => x.Status == InviteStatus.Declined)
                .Count();

            var participantsWithPendingInvites = eventParticipants
                .Where(x => x.Status == InviteStatus.Invited)
                .Count();


            var particpantsWithInvites = eventParticipants.Count();

            currentEventStatistic = particpantsWithInvites == 0 ? 0 : (double)participantsWithAcceptedInvites / particpantsWithInvites;

            var eventViewModel = new EventViewModel
            {
                Id = @event.Id,
                Description = @event.Description,
                Location = @event.Location,
                OwnerUserId = @event.OwnerUserId,
                StartDate = @event.StartDate,
                Status = @event.Status,
                Title = @event.Title!,
                Participants = eventParticipants
            };

            res.EventStatistics.Add(new EventStatistic
            {
                EventId = @event.Id,
                Event = eventViewModel,
                AcceptedInvitesCount = participantsWithAcceptedInvites,
                DeclinedInvitesCount = participantsWithDeclinedInvites,
                PendingInvitesCount = participantsWithPendingInvites,
                TotalInvitedCount = particpantsWithInvites,
                CalculatedStatsticResult = currentEventStatistic
            });
        }

        return res;
    }

    /// <summary>Извлича всички събития от базата данни, като прилага зададени филтри.</summary>
    /// <param name="req">Обект от тип <see cref="GetEventsWithFiltersRequest"/>, съдържащ критерии за филтриране</param>
    /// <returns>Обект от тип <see cref="GetAllEventsResponse"/>,</returns>
    public async Task<GetAllEventsResponse> GetEventsWithFilters(GetEventsWithFiltersRequest req)
    {
        var response = new GetAllEventsResponse();

        var query = _db.Events.AsQueryable().Where(x=> x.Status == EventStatus.Active);

        if (!string.IsNullOrEmpty(req.Title))
        {
            query = query.Where(e => e.Title!.Contains(req.Title));
        }

        if (req.StartDate.HasValue)
        {
            query = query.Where(e => e.StartDate == req.StartDate.Value.Date);
        }

        if (!string.IsNullOrEmpty(req.Location))
        {
            query = query.Where(e => e.Location!.Contains(req.Location));
        }

        var filteredEvents = await query.AsNoTracking().ToListAsync();

        var eventSummaries = filteredEvents.Select(e => new EventSummary
        {
            Id = e.Id,
            Title = e.Title!,
            Description = e.Description,
            StartDate = e.StartDate,
            OwnerUserId = e.OwnerUserId,
            Status = e.Status,
            Location = e.Location,
            Participants = e.Participants
        }).ToList();

        response.Events = eventSummaries;

        return response;
    }
}