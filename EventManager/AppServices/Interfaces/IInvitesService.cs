using EventManager.AppServices.Messaging;
using EventManager.AppServices.Messaging.Requests.InvitesRequests;
using EventManager.AppServices.Messaging.Responses.InvitesResponses;

namespace EventManager.AppServices.Interfaces
{
    public interface IInvitesService
    {

        Task<GetInvitesIncomingResponse> LoadInvitesIncomingAsync(GetInvitesIncomingRequest req);

        Task<GetInvitesOutcomingResponse> LoadInvitesOutcomingAsync(GetInvitesOutcomingRequest req);


        Task<ServiceResponseBase> AcceptInviteAsync(Guid inviteId, Guid actingUserId);



        Task<ServiceResponseBase> DeclineInviteAsync(Guid inviteId, Guid actingUserId);

    }
}
