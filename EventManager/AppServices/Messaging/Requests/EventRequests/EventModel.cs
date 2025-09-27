using System.ComponentModel.DataAnnotations;

namespace EventManager.AppServices.Messaging.Requests.EventRequests
{
    public class EventModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Името е задължително.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Местоположението е задължително.")]
        public string? Location { get; set; }


        [Required(ErrorMessage = "Бележка полето е задължително.")]
        public string Description { get; set; } = "Събитието е в процес на изготвяне";

        [Required(ErrorMessage = "Дата полето е задължително.")]
        public DateTime StartDate { get; set; }

        public bool Public { get; set; } = true;

        public Data.Enums.EventStatus Status { get; set; }
        public Guid OwnerUserId { get; set; }
    }
}
