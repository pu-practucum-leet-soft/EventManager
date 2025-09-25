using EventManager.AppServices.Interfaces;
using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.EventRequests;
using EventManager.AppServices.Messaging.Requests.InvitesRequests;
using EventManager.AppServices.Messaging.Responses.EventResponses;
using EventManager.AppServices.Messaging.Responses.InvitesResponses;
using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Contexts;
using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventManager.AppServices.Implementations
{
    public class InvitesService : IInvitesService
    {


        private readonly EventManagerDbContext _context;
        private readonly ILogger<InvitesService> _logger;

        public InvitesService(ILogger<InvitesService> logger, EventManagerDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GetInvitesIncomingResponse> LoadInvitesIncomingAsync(GetInvitesIncomingRequest req)
        {
            var response = new GetInvitesIncomingResponse();

            try
            {
                var q = _context.EventParticipants
                    .AsNoTracking()
                    .Where(p => p.InviteeId == req.ActingUserId); // входящи покани

                var page = req.Page < 1 ? 1 : req.Page;
                var pageSize = req.PageSize < 1 ? 20 : req.PageSize;

                response.Total = await q.CountAsync();

                var items = await q
                    .OrderByDescending(p => p.Event.StartDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new InviteViewModel
                    {
                        EventId = p.EventId,
                        EventTitle = p.Event.Title,
                        Inviter = new UserViewModel
                        {
                            UserId = p.InviterId,
                            FirstName = p.Inviter!.FirstName,
                            LastName = p.Inviter!.LastName,
                            Email = p.Inviter!.Email
                        },
                        Invitee = new UserViewModel
                        {
                            UserId = p.InviteeId,
                            FirstName = p.Invitee!.FirstName,
                            LastName = p.Invitee!.LastName,
                            Email = p.Invitee!.Email
                        },
                        InviteStatus = p.inviteStatus
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
                    .Where(p => p.InviterId == req.ActingUserId); // OUTGOING: аз съм поканил

                // по желание: страниране
                var page = req.Page < 1 ? 1 : req.Page;
                var pageSize = req.PageSize < 1 ? 20 : req.PageSize;

                response.Total = await q.CountAsync();

                var items = await q
                    .OrderByDescending(p => p.Event.StartDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new InviteViewModel
                    {
                        EventId = p.EventId,
                        EventTitle = p.Event.Title,
                        Invitee = new UserViewModel
                        {
                            UserId = p.InviteeId,
                            FirstName = p.Invitee!.FirstName,
                            LastName = p.Invitee!.LastName,
                            Email = p.Invitee!.Email
                        },
                        Inviter = new UserViewModel
                        {
                            UserId = p.InviterId,
                            FirstName = p.Inviter!.FirstName,
                            LastName = p.Inviter!.LastName,
                            Email = p.Inviter!.Email
                        },
                        InviteStatus = p.inviteStatus
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
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.InviteeId == actingUserId);

                if (invite == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    return response;
                }

                if (invite.inviteStatus != InviteStatus.Invited)
                {
                    response.StatusCode = BusinessStatusCodeEnum.BadRequest;
                    response.Message = "Поканата вече е обработена.";
                    return response;
                }

                invite.inviteStatus = InviteStatus.Accepted;
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

            try
            {
                var invite = await _context.EventParticipants
                    .FirstOrDefaultAsync(p => p.EventId == eventId && p.InviteeId == actingUserId);

                if (invite == null)
                {
                    response.StatusCode = BusinessStatusCodeEnum.NotFound;
                    return response;
                }

                if (invite.inviteStatus != InviteStatus.Invited)
                {
                    response.StatusCode = BusinessStatusCodeEnum.BadRequest;
                    response.Message = "Поканата вече е обработена.";
                    return response;
                }

                invite.inviteStatus = InviteStatus.Declined;
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



    }
}
