using EventManager.AppServices.Interfaces;
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
/// Serves to contain the business logic for handling the events.
/// </summary>
public class EventService : IEventService
{
    private readonly EventManagerDbContext _db;
    private readonly ILogger<EventService> _logger;

    /// <summary>
    /// The EventService constructor serves to instantiate the service and inject the needed dependencies.
    /// </summary>
    public EventService(EventManagerDbContext db, ILogger<EventService> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// The CreateEvent method delivers customer data from the client side uses it to instatiate the Event entity that is being persisted into the storage.
    /// </summary>
    /// <param name="req">
    /// The CreateEventRequest is a view model that contais user defined data with details about a new event. 
    /// It acts as a mediator for transferring the creation data.
    /// </param> 
    /// <param name="userId">
    /// The userId parameter is extracted from the UserManager service in the Controller Action and passed to the method.
    /// </param>
    /// <returns>The CreateEventResponse view model containing the status of the request and a message to track the process.</returns>
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

    /// <summary>
    /// Provides access to the database and queries for a single event called by the client.
    /// </summary>
    /// <param name="req">
    /// Contains the Event Id used to query the Events table.
    /// </param> 
    /// <returns>EventViewModel that contains the main event data along with the contact information of the event owner.</returns>
    /// <response code="200">Returns the requested project board.</response>
    /// <response code="404">If the project board is not found.</response>
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
    /// Provides access to an event and changes the contents of the event data.
    /// </summary>
    /// <param name="req">
    /// Conains the event data to be passed to the database entity.
    /// </param> 
    /// <returns>EditEventResponse returns a message signifying the completion of the request.</returns>
    /// <response code="200">Returns the requested project board.</response>
    /// <response code="404">If the project board is not found.</response>
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

    /// <summary>
    /// Gets an event from the database and invites a participant.
    /// </summary>
    /// <param name="req">
    /// Sends data from the client about the participants who are to be invited to the event.
    /// </param> 
    /// <param name="inviterId">
    /// Contains the Id of the user who is sends the invites.
    /// </param> 
    /// <returns>The response contains the number of invited users.</returns>
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
    /// The method calls all created events and returns them to the client.
    /// </summary>
    /// <returns>The GetAllEventsResponse returns the collected events from the database.</returns>
    public async Task<GetAllEventsResponse> GetAllEvents()
    {
        var response = new GetAllEventsResponse();

        var q = await _db.Events.AsNoTracking().ToListAsync();

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
        }).ToList();

        response.Events = allEventsList;

        return response;
    }


    /// <summary>
    /// Calculates the a statistic about events' count and the percentage of participants who accepted their invites.
    /// </summary>
    /// <param name="ownerId">
    /// A Guid of the owner of the event/s
    /// </param> 
    /// <returns>The view modal with the calculated data and additional information.</returns>
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
}