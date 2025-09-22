using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.AppServices.Implementations;

public class EventService : IEventService
{
    private readonly EventManagerDbContext _db;
    public EventService(EventManagerDbContext db) => _db = db;

    public CreateEventResponse CreateEvent(CreateEventRequest req)
    {
        var ev = new Event
        {
            Name = req.Name,
            Location = req.Location,
            Notes = req.Notes,
            OwnerUserId = req.OwnerUserId
        };
        _db.Events.Add(ev);
        _db.SaveChanges();
        // owner auto-joins  evetn 
        _db.EventParticipants.Add(new EventParticipant { EventId = ev.Id, UserId = req.OwnerUserId });
        _db.SaveChanges();
        return new CreateEventResponse { EventId = ev.Id };
    }
    
    public EventViewModel GetEvent(GetEventRequest req)
    {
        var ev = _db.Events
            .Include(e => e.Participants)
            .FirstOrDefault(e => e.Id == req.EventId);

        if (ev == null) throw new KeyNotFoundException("Event not found");

        var participantIds = ev.Participants.Select(p => p.UserId).ToList();
        return new EventViewModel
        {
            Id = ev.Id,
            Name = ev.Name,
            Location = ev.Location,
            Notes = ev.Notes,
            OwnerUserId = ev.OwnerUserId,
            ParticipantUserIds = participantIds
        };
    }

    public EditEventResponse EditEvent(EditEventRequest req)
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

    public AddParticipantsResponse AddParticipants(AddParticipantsRequest req)
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

    public GetAllEventsResponse GetAllEvents(GetAllEventsRequest req)
    {
        var q = _db.Events.AsQueryable();
        if (req.OwnerUserId.HasValue)
            q = q.Where(e => e.OwnerUserId == req.OwnerUserId.Value);

        var list = q.Select(e => new EventSummary
        {
            Id = e.Id,
            Name = e.Name,
            Location = e.Location
        }).ToList();

        return new GetAllEventsResponse { Events = list };
    }
}
