using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventManager.AppServices.Implementations;

public class EventService : IEventService
{
    private readonly EventManagerDbContext _db;
    public EventService(EventManagerDbContext db) => _db = db;

    public async Task<CreateEventResponse> CreateEvent([FromBody] CreateEventRequest req)
    {
        var res = new CreateEventResponse();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        if (!Guid.TryParse(userId, out var ownerId))
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.Unauthorized;
            return res;
        }

        try
        {
            var ev = new Event
            {
                Name = req.Name,
                Location = req.Location,
                Notes = req.Notes,
                StartDate = req.StartDate,
                OwnerUserId = ownerId 
            };

            _db.Events.Add(ev);
            // owner auto-joins
            _db.EventParticipants.Add(new EventParticipant { EventId = ev.Id, UserId = ownerId });

            _db.SaveChanges();



            return res;
        }
        catch (DbUpdateException ex)
        {
            res.StatusCode = Messaging.BusinessStatusCodeEnum.InternalServerError;
            /// here is problem
            // await tx.RollbackAsync(ct);
            // _logger.LogError(ex, "CreateEvent DbUpdateException for owner {OwnerId}", ownerId);
            // return StatusCode(500, "Database error while creating event.");

            return res;
        }
    }

    public async Task<EventViewModel> GetEvent(GetEventRequest req)
    {
        var ev = _db.Events
            .Include(e => e.Participants)
            .FirstOrDefault(e => e.Id == req.EventId);

        if (ev == null) throw new KeyNotFoundException("Event not found");

        var participantIds = ev.Participants.Select(p => p.UserId).ToList();
        var owner = new UserViewModel
        {
            FirstName = ev.OwnerUser.FirstName,
            LastName = ev.OwnerUser.LastName,
            Email = ev.OwnerUser.Email,
        };

        return new EventViewModel
        {
            Id = ev.Id,
            Name = ev.Name,
            Location = ev.Location,
            Notes = ev.Notes,
            Owner = owner,
            ParticipantUserIds = participantIds
        };
    }

    public async Task<EditEventResponse> EditEvent(EditEventRequest req)
    {
        var ev = _db.Events.FirstOrDefault(e => e.Id == req.EventId);
        if (ev == null) throw new KeyNotFoundException("Event not found");
        if (ev.OwnerUserId != req.ActorUserId) throw new UnauthorizedAccessException("Only owner can edit");

        if (req.Name is not null) ev.Name = req.Name;
        if (req.Location is not null) ev.Location = req.Location;
        if (req.Notes is not null) ev.Notes = req.Notes;

        _db.SaveChanges();
        return new EditEventResponse { Success = true };
    }

    public async Task<AddParticipantsResponse> AddParticipants(AddParticipantsRequest req)
    {
        var ev = _db.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == req.EventId);
        if (ev == null) throw new KeyNotFoundException("Event not found");
        if (ev.OwnerUserId != req.ActorUserId) throw new UnauthorizedAccessException("Only owner can add participants");

        var existing = ev.Participants.Select(p => p.UserId).ToHashSet();
        int added = 0;
        foreach (var uid in req.UserIds)
        {
            if (!existing.Contains(uid))
            {
                _db.EventParticipants.Add(new EventParticipant { EventId = ev.Id, UserId = uid });
                added++;
            }
        }
        _db.SaveChanges();
        return new AddParticipantsResponse { Added = added };
    }

    public async Task<GetAllEventsResponse> GetAllEvents()
    {
        // Get the user's Id from Claims and parse it to a Guid
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid ownerId);

        //var q = _db.Events.Where(e => e.OwnerUserId == ownerId);

        //var list = q.Select(e => new EventSummary
        //{
        //    Id = e.Id,
        //    Name = e.Name,
        //    Location = e.Location
        //}).ToList();

        var res = new List<EventSummary>();

        var response = new GetAllEventsResponse();
        response.Events = res;

        res.Add(new EventSummary { Name = "eventData1", Location = "Sofia", Notes = "asd", ParticipantsCount = new List<string> { "1", "2", "3", "5", "12" }.Count });
        res.Add(new EventSummary { Name = "eventData2", Location = "Sofia", Notes = "afsda", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2" }.Count });
        res.Add(new EventSummary { Name = "eventData3", Location = "Plovdiv", Notes = "asdgwefw", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2", "2", "2", "2", "2" }.Count });
        res.Add(new EventSummary { Name = "eventData4", Location = "Plovdiv", Notes = "asdwd", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2", "2", "2", "2", "2" }.Count });
        res.Add(new EventSummary { Name = "eventData5", Location = "Varna", Notes = "asdfsadg", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2", "2", "2" }.Count });
        res.Add(new EventSummary { Name = "eventData6", Location = "Varna", Notes = "asdgs", ParticipantsCount = new List<string> { "1", "2", "3", "2", "2" }.Count });
        res.Add(new EventSummary { Name = "eventData7", Location = "Plovdiv", Notes = "swef", ParticipantsCount = new List<string> { "1", "2", "3" }.Count });

        return response;
    }
}
