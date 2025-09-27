namespace EventManager.AppServices.Messaging.Responses.InvitesResponses
{
    public class GetInvitesOutcomingResponse : ServiceResponseBase
    {
        public List<InviteViewModel> Invites { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
