


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
/// Сървис, който предоставя функционалности за началната страница на приложението.
/// Използва базата данни за извличане на предстоящи събития, скорошни събития и покани
/// за даден потребител.
/// </summary>
public class HomeService : IHomeService
{
    private readonly EventManagerDbContext _db;
    private readonly ILogger<IHomeService> _logger;

    /// <summary>
    /// Конструктор за инициализиране на <see cref="HomeService"/>.
    /// </summary>
    /// <param name="db">Контекст за достъп до базата данни <see cref="EventManagerDbContext"/>.</param>
    /// <param name="logger">Логър за проследяване на събития и грешки.</param>
    public HomeService(EventManagerDbContext db, ILogger<IHomeService> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>Извлича данните за началната страница на даден потребител</summary>
    /// <param name="userId">Уникалният идентификатор на потребителя (<see cref="Guid"/>).</param>
    /// <returns>Обект от тип <see cref="GetHomeResponse"/></returns>
    public async Task<GetHomeResponse> GetHome(Guid userId)
    {
        var upcomingEvents = await _db.EventParticipants
            .Where(e => e.InviteeId == userId
            && e.Status == InviteStatus.Accepted
            && e.Event!.StartDate > DateTime.UtcNow
            && e.Event.Status == EventStatus.Active)
            .Select(e => new EventViewModel
            {
                Id = e.Event!.Id,
                Title = e.Event.Title!,
                Description = e.Event.Description,
                Location = e.Event.Location,
                StartDate = e.Event.StartDate,
                Status = e.Event.Status,
                OwnerUserId = e.Event.OwnerUserId
            })
            .ToArrayAsync();

        var recentEventsQuery = await _db.Events
                            .Where(e => e.StartDate >= DateTime.UtcNow && e.Status == EventStatus.Active)
                            .Select(e => new EventViewModel
                            {
                                Id = e.Id,
                                Title = e.Title!,
                                Description = e.Description,
                                Location = e.Location,
                                StartDate = e.StartDate,
                                Status = e.Status,
                                OwnerUserId = e.OwnerUserId
                            })
                            .ToArrayAsync();
        
        var recentEvents = recentEventsQuery.Reverse().ToArray();
        var invites = await _db.EventParticipants
            .Where(ep => ep.InviteeId == userId && ep.Status == InviteStatus.Invited)
            .Include(ep => ep.Event)
            .Include(ep => ep.Inviter)
            .Select(ep => new EventParticipantViewModel
            {
                Id = ep.Id,
                Event = new EventViewModel
                {
                    Id = ep.Event!.Id,
                    Title = ep.Event.Title!,
                    Description = ep.Event.Description,
                    Location = ep.Event.Location,
                    StartDate = ep.Event.StartDate,
                    Status = ep.Event.Status,
                    OwnerUserId = ep.Event.OwnerUserId
                },
                Inviter = new UserViewModel
                {
                    UserName = ep.Inviter!.UserName,
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