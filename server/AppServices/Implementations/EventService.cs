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

public class EventService : IEventService
{
    private readonly EventManagerDbContext _db;
    private readonly ILogger<EventService> _logger;
    public EventService(EventManagerDbContext db, ILogger<EventService> logger)
    {
        _db = db;
        _logger = logger;
    }

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
                Status = Data.Enums.EventStatus.Active
            };

            await _db.Events.AddAsync(ev);
            // owner auto-joins
            await _db.EventParticipants.AddAsync(new EventParticipant { EventId = ev.Id, InviteeId = ownerId });

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

    public async Task<EventViewModel> GetEvent(GetEventRequest req)
    {
        var ev = await _db.Events
            .AsNoTracking()
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == req.EventId);

        if (ev == null) throw new KeyNotFoundException("Event not found");

        var participantIds = ev.Participants.ToList();
        var owner = new UserViewModel
        {
            UserName = ev.OwnerUser.UserName,
            Email = ev.OwnerUser.Email,
        };

        return new EventViewModel
        {
            Id = ev.Id,
            Title = ev.Title,
            Description = ev.Description,
            StartDate = ev.StartDate,
            Location = ev.Location,
            OwnerUserId = ev.OwnerUserId,
            Participants = ev.Participants,
            Status = ev.Status
        };
    }

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
            UserName = ev.OwnerUser.UserName,
            Email = ev.OwnerUser.Email,
        };

        res.Event = new EventViewModel
        {
            Id = ev.Id,
            Title = ev.Title,
            Description = ev.Description,
            StartDate = ev.StartDate,
            Location = ev.Location,
            OwnerUserId = ev.OwnerUserId,
            Participants = participants,
            Status = ev.Status
        };

        return res;
     }

    public async Task<EditEventResponse> EditEvent(EditEventRequest req)
    {
        var ev = await _db.Events.FirstOrDefaultAsync(e => e.Id == req.EventId);

        if (ev == null) throw new KeyNotFoundException("Event not found");

        if (ev.OwnerUserId != req.ActorUserId) throw new UnauthorizedAccessException("Only owner can edit");

        if (req.Title is not null) ev.Title = req.Title;
        if (req.Location is not null) ev.Location = req.Location;
        if (req.Description is not null) ev.Description = req.Description;
        // the state of the properties below will be checked and ensured to have values in the FE
        ev.StartDate = req.StartDate;
        ev.Status = req.Status;

        await _db.SaveChangesAsync();
        return new EditEventResponse { Success = true };
    }

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

    public async Task<GetAllEventsResponse> GetAllEvents()
    {
        var response = new GetAllEventsResponse();

        var q = await _db.Events.AsNoTracking().ToListAsync();

        var allEventsList = q.Select(e => new EventSummary
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            StartDate = e.StartDate,
            OwnerUserId = e.OwnerUserId,
            Status = e.Status,
            Location = e.Location,
            Participants = e.Participants
        }).ToList();

        response.Events = allEventsList;

        //res.Add(new EventSummary { Name = "eventData1", Location = "Sofia", Description = "asd", ParticipantsCount = new List<string> { "1", "2", "3", "5", "12" }.Count });
        //res.Add(new EventSummary { Name = "eventData2", Location = "Sofia", Description = "afsda", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2" }.Count });
        //res.Add(new EventSummary { Name = "eventData3", Location = "Plovdiv", Description = "asdgwefw", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2", "2", "2", "2", "2" }.Count });
        //res.Add(new EventSummary { Name = "eventData4", Location = "Plovdiv", Description = "asdwd", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2", "2", "2", "2", "2" }.Count });
        //res.Add(new EventSummary { Name = "eventData5", Location = "Varna", Description = "asdfsadg", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2", "2", "2" }.Count });
        //res.Add(new EventSummary { Name = "eventData6", Location = "Varna", Description = "asdgs", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2" }.Count });
        //res.Add(new EventSummary { Name = "eventData7", Location = "Plovdiv", Description = "swef", ParticipantsCount = new List<string> { "1", "2", "3" }.Count });

        return response;
    }

    //Общ брой събития, създадени от потребителя.
    //Процент участници, потвърдили участие("Потвърден") спрямо всички поканени.
    public async Task<StatisticViewModel> GetEventStatistic(Guid ownerId)
    {
        var ownerEvents = await _db.Events
            .AsNoTracking()
            .Where(x => x.OwnerUserId == ownerId)
            .ToListAsync();

        var res = new StatisticViewModel();
        // Total count of events created by the uuser.
        res.OwnerEventsCount = ownerEvents.Count();

        foreach ( var @event in ownerEvents)
        {
            // Calculate percentage of "Accepted" invites against all "Invited".
            var eventParticipants = @event.Participants.ToList();

            var participantsWithAcceptedInvites = eventParticipants
                .Where(x => x.Status == InviteStatus.Accepted)
                .Count();

            var particpantsWithInvites = eventParticipants.Count();

            var currentEventStastic = participantsWithAcceptedInvites / particpantsWithInvites;

            res.EventStatistics.Add(new EventStatistic
            { 
                EventId = @event.Id,
                AcceptedInvitesCount = participantsWithAcceptedInvites,
                TotalInvitedCount = particpantsWithInvites,
                CalculatedStatsticResult = currentEventStastic
            });
        }

        return res;
    }
}