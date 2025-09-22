using EventManager.AppServices.Messaging.Requests.PetRequests;

namespace EventManager.AppServices.Messaging.Responses.PetResponses
{
    public class PetViewModel
    {
   
        public string Name { get; set; }
        public string? Type { get; set; }
        public int Age { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhotoUrl { get; set; }
        public Guid UserId { get; set; }

        
    }
}
