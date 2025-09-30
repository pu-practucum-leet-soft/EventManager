using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Requests.InvitesRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.InvitesResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Entities;
using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventManager.AppServices.Implementations
{
    public class InvitesService : IInvitesService
    {


        private readonly EventManagerDbContext _context;
        private readonly ILogger<InvitesService> _logger;

        public InvitesService(EventManagerDbContext db, ILogger<InvitesService> logger)
        {
            _logger = logger;
            _context = db;
        }

        public async Task<GetInvitesAllResponse> LoadAllInvitesAsync(Guid userId)
        {
            var response = new GetInvitesAllResponse();

            try
            {
                var incomingReq = new GetInvitesIncomingRequest
                {
                    ActingUserId = userId,
                    Page = 1,
                    PageSize = int.MaxValue // зареждаме всички
                };

                var incomingRes = await LoadInvitesIncomingAsync(incomingReq);
                if (incomingRes.StatusCode != BusinessStatusCodeEnum.Success)
                {
                    response.IncomingInvites = new List<EventParticipantViewModel>();
                }
                else
                {
                    response.IncomingInvites = incomingRes.Invites;
                }

                var outcomingReq = new GetInvitesOutcomingRequest
                {
                    ActingUserId = userId,
                    Page = 1,
                    PageSize = int.MaxValue // зареждаме всички
                };

                var outcomingRes = await LoadInvitesOutcomingAsync(outcomingReq);
                if (outcomingRes.StatusCode != BusinessStatusCodeEnum.Success)
                {
                    response.OutcomingInvites = new List<EventParticipantViewModel>();
                }
                else
                {
                    response.OutcomingInvites = outcomingRes.Invites;
                }

                response.StatusCode = BusinessStatusCodeEnum.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all invites for user {UserId}", userId);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
            }

            return response;
        }
        public async Task<GetInvitesIncomingResponse> LoadInvitesIncomingAsync(GetInvitesIncomingRequest req)
        {
            var response = new GetInvitesIncomingResponse();

            try
            {
                var q = _context.EventParticipants
                    .AsNoTracking()
                    .Where(p => p.InviteeId == req.ActingUserId && p.Status == InviteStatus.Invited); // входящи покани

                var page = req.Page < 1 ? 1 : req.Page;
                var pageSize = req.PageSize < 1 ? 20 : req.PageSize;

                response.Total = await q.CountAsync();

                var items = await q
                    .OrderByDescending(p => p.Event.StartDate)
                    .Where(p => p.Event.Status == EventStatus.Active)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new EventParticipantViewModel
                    {
                        Event = new EventViewModel
                        {
                            Id = p.EventId,
                            Title = p.Event.Title,
                            Description = p.Event.Description,
                            StartDate = p.Event.StartDate,
                            Location = p.Event.Location,
                            Status = p.Event.Status
                        },
                        Inviter = new UserViewModel
                        {
                            Id = p.InviterId,
                            UserName = p.Inviter!.UserName,
                            Email = p.Inviter!.Email,
                        },
                        Invitee = new UserViewModel
                        {
                            Id = p.InviteeId,
                            UserName = p.Invitee!.UserName,
                            Email = p.Invitee!.Email,
                        },
                        Status = p.Status
                    })
                    .ToListAsync();

                response.Invites = items;
                response.Page = page;
                response.PageSize = pageSize;
                response.StatusCode = BusinessStatusCodeEnum.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading incoming invites for user {UserId}", req.ActingUserId);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                response.Message = "Възникна грешка при зареждане на входящите покани.";
            }

            return response;
        }


        public async Task<GetInvitesOutcomingResponse> LoadInvitesOutcomingAsync(GetInvitesOutcomingRequest req)
        {

            var response = new GetInvitesOutcomingResponse();

            try
            {
                var q = _context.EventParticipants
                    .AsNoTracking()
                    .Where(p => p.InviterId == req.ActingUserId && p.InviterId != p.InviteeId); // OUTGOING: аз съм поканил

                // по желание: страниране
                var page = req.Page < 1 ? 1 : req.Page;
                var pageSize = req.PageSize < 1 ? 20 : req.PageSize;

                response.Total = await q.CountAsync();

                var items = await q
                    .OrderByDescending(p => p.Event.StartDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new EventParticipantViewModel
                    {
                        Event = new EventViewModel
                        {
                            Id = p.EventId,
                            Title = p.Event.Title,
                            Description = p.Event.Description,
                            StartDate = p.Event.StartDate,
                            Location = p.Event.Location,
                            Status = p.Event.Status
                        },
                        Inviter = new UserViewModel
                        {
                            Id = p.InviterId,
                            UserName = p.Inviter!.UserName,
                            Email = p.Inviter!.Email
                        },
                        Invitee = new UserViewModel
                        {
                            Id = p.InviteeId,
                            UserName = p.Invitee!.UserName,
                            Email = p.Invitee!.Email
                        },
                        Status = p.Status
                    })
                    .ToListAsync();

                response.Invites = items;
                response.Page = page;
                response.PageSize = pageSize;
                response.StatusCode = BusinessStatusCodeEnum.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading outgoing invites for user {UserId}", req.ActingUserId);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                response.Message = "Възникна грешка при зареждане на изходящите покани.";
            }

            return response;
        }



        public async Task<ServiceResponseBase> AcceptInviteAsync(Guid eventId, Guid actingUserId)
        {
            var response = new ServiceResponseBase();

            try
            {
                var invite = await _context.EventParticipants
                    .FirstOrDefaultAsync(p => (p.EventId == eventId) && (p.InviteeId == actingUserId));

                if (invite == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    response.Message = "" + actingUserId + " -------------- " + eventId;
                    return response;
                }

                if (invite.Status != InviteStatus.Invited)
                {
                    response.StatusCode = BusinessStatusCodeEnum.BadRequest;
                    response.Message = "Поканата вече е обработена.";
                    return response;
                }

                invite.Status = InviteStatus.Accepted;
                _context.EventParticipants.Update(invite);
                await _context.SaveChangesAsync();

                response.StatusCode = BusinessStatusCodeEnum.Success;
                response.Message = "Поканата е приета успешно.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while accepting invite {EventId} for user {UserId}", eventId, actingUserId);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                response.Message = "Възникна грешка при приемане на поканата.";
            }

            return response;
        }

        public async Task<ServiceResponseBase> DeclineInviteAsync(Guid eventId, Guid actingUserId)
        {
            var response = new ServiceResponseBase();

            Console.WriteLine("DeclineInviteAsync called with eventId: " + eventId + " and actingUserId: " + actingUserId);
            try
            {
                var invite = await _context.EventParticipants
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.InviteeId == actingUserId);

                if (invite == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    return response;
                }

                if (invite.Status != InviteStatus.Invited)
                {
                    response.StatusCode = BusinessStatusCodeEnum.BadRequest;
                    response.Message = "Поканата вече е обработена.";
                    return response;
                }

                invite.Status = InviteStatus.Declined;
                _context.EventParticipants.Update(invite);
                await _context.SaveChangesAsync();

                response.StatusCode = BusinessStatusCodeEnum.Success;
                response.Message = "Поканата е отказана.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while accepting invite {EventId} for user {UserId}", eventId, actingUserId);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                response.Message = "Възникна грешка при приемане на поканата.";
            }

            return response;
        }


        public async Task<ServiceResponseBase> CreateInviteAsync(Guid eventId, Guid inviterId, string inviteeEmail)
        {
            var response = new ServiceResponseBase();

            try
            {
                var ev = await _context.Events
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == eventId && e.Status == EventStatus.Active);

                if (ev == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    response.Message = "Събитието не е намерено.";
                    return response;
                }

                var inviter = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == inviterId);

                if (inviter == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    response.Message = "Потребителят поканител не е намерен.";
                    return response;
                }

                var invitee = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == inviteeEmail);

                if (invitee == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    response.Message = "Потребителят поканен не е намерен.";
                    return response;
                }

                var attending = await _context.EventParticipants
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.Invitee.Email == inviteeEmail && p.Status == InviteStatus.Accepted);

                if (attending != null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.Conflict;
                    response.Message = "Потребителят вече е участник в събитието.";
                    return response;
                }

                var existing = await _context.EventParticipants
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.Invitee.Email == inviteeEmail);

                //? Идеята е да ограничим други потребители да не могат да изпращат покани на един и същ човек за едно и също събитие
                //? Но да позволим на потребител да си изпрати покана сам на себе си (т.е. да се запише като участник)
                if (existing != null)
                {
                    if (invitee.Id == inviterId)
                    {
                        Console.WriteLine("User is inviting themselves, auto-accepting the invite.");
                        _context.EventParticipants.Update(existing);
                        existing.Status = InviteStatus.Accepted;

                        await _context.SaveChangesAsync();
                        response.StatusCode = BusinessStatusCodeEnum.Success;
                        response.Message = "Вече сте участник в събитието.";
                        return response;
                    }
                    
                    response.StatusCode = BusinessStatusCodeEnum.Conflict;
                    response.Message = "Поканата вече съществува.";
                    return response;
                }
                var invite = new EventParticipant
                {
                    EventId = eventId,
                    InviterId = inviterId,
                    InviteeId = invitee.Id,
                    Status = inviterId == invitee.Id ? InviteStatus.Accepted : InviteStatus.Invited
                };
                    _context.EventParticipants.Add(invite);
                    await _context.SaveChangesAsync();

                    response.StatusCode = BusinessStatusCodeEnum.Success;
                    response.Message = "Поканата е изпратена успешно.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    _logger.LogError(ex, "Error while creating invite {EventId} from user {InviterId} to user {InviteeEmail}", eventId, inviterId, inviteeEmail);
                    response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                    response.Message = "Възникна грешка при изпращане на поканата.";
                }

                return response;
        }

        public async Task<ServiceResponseBase> UnattendEventAsync(Guid eventId, Guid userId)
        {
            var response = new ServiceResponseBase();

            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.Unauthorized;
                    response.Message = "Потребителят не е автентифициран.";
                    return response;
                }

                var invites = await _context.EventParticipants
                    .Where(p => p.InviteeId == userId && p.Status == InviteStatus.Accepted && p.EventId == eventId)
                    .ToListAsync();

                if (invites.Count == 0)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    response.Message = "Няма намерени покани за отказване.";
                    return response;
                }

                
                _context.EventParticipants.UpdateRange(invites);
                foreach (var invite in invites)
                {
                    invite.Status = InviteStatus.Declined;
                }

                await _context.SaveChangesAsync();

                response.StatusCode = BusinessStatusCodeEnum.Success;
                response.Message = "Всички покани са отказани успешно.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while cancelling invites for user {UserId}", userId);
                response.StatusCode = BusinessStatusCodeEnum.InternalServerError;
                response.Message = "Възникна грешка при отказване на поканите.";
            }

            return response;
        }
    }
}
