


using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.HomeResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventManager.AppServices.Implementations;

/// <summary>
/// Service for home page functionality
/// </summary>
public class HomeService : IHomeService
{
    private readonly EventManagerDbContext _db;
    private readonly ILogger<IHomeService> _logger;

    public HomeService(EventManagerDbContext db, ILogger<IHomeService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<GetHomeResponse> GetHome(Guid userId)
    {
        var upcomingEvents = await _db.Events
            .Where(e => e.OwnerUserId == userId)
            .Select(e => new EventViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartDate = e.StartDate,
                Status = e.Status,
                OwnerUserId = e.OwnerUserId
            })
            .ToArrayAsync();

        var recentEvents = await _db.Events
            .Where(e => e.StartDate < DateTime.UtcNow)
            .Select(e => new EventViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartDate = e.StartDate,
                Status = e.Status,
                OwnerUserId = e.OwnerUserId
            })
            .ToArrayAsync();

        var invites = await _db.EventParticipants
            .Where(ep => ep.InviteeId == userId && ep.Status == InviteStatus.Invited)
            .Include(ep => ep.Event)
            .Include(ep => ep.Inviter)
            .Select(ep => new EventParticipantViewModel
            {
                Id = ep.Id,
                Event = new EventViewModel
                {
                    Id = ep.Event.Id,
                    Title = ep.Event.Title,
                    Description = ep.Event.Description,
                    Location = ep.Event.Location,
                    StartDate = ep.Event.StartDate,
                    Status = ep.Event.Status,
                    OwnerUserId = ep.Event.OwnerUserId
                },
                Inviter = new UserViewModel
                {
                    UserName = ep.Inviter.UserName,
                    Email = ep.Inviter.Email

                },
                Status = ep.Status
            })
            .ToArrayAsync();

        return new GetHomeResponse
        {
            UpcomingEvents = upcomingEvents,
            RecentEvents = recentEvents,
            Invites = invites
        };
    }
}