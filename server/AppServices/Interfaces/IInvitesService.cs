using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.InvitesRequests;
using EventManager.AppServices.Messaging.Responses.InvitesResponses;

namespace EventManager.AppServices.Interfaces
{
    public interface IInvitesService
    {

        Task<GetInvitesAllResponse> LoadAllInvitesAsync(Guid userId);
        Task<GetInvitesIncomingResponse> LoadInvitesIncomingAsync(GetInvitesIncomingRequest req);

        Task<GetInvitesOutcomingResponse> LoadInvitesOutcomingAsync(GetInvitesOutcomingRequest req);


        Task<ServiceResponseBase> AcceptInviteAsync(Guid inviteId, Guid actingUserId);



        Task<ServiceResponseBase> DeclineInviteAsync(Guid inviteId, Guid actingUserId);

        Task<ServiceResponseBase> CreateInviteAsync(Guid eventId, Guid inviterId, Guid inviteeId );

    }
}
