using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Entities;

namespace EventManager.AppServices.Messaging.Responses.InvitesResponses
{
    public class InviteViewModel
    {


        public Guid EventId { get; set; }

        public string EventTitle { get; set; }

        public UserViewModel Inviter { get; set; }

        public UserViewModel Invitee { get; set; }

        public bool Join {  get; set; }

        public Data.Enums.InviteStatus InviteStatus { get; set; }
    }
}
