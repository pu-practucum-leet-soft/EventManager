using System.ComponentModel.DataAnnotations;

namespace EventManager.AppServices.Messaging.Requests.PetRequests
{
    public class UpdatePetRequest
    {
        [Required]
        public string Name { get; set; }

        public string? Type { get; set; }

        public int Age { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
