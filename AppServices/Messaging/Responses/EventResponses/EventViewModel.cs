namespace EventManager.AppServices.Messaging.Responses.EventResponses;

using System.ComponentModel.DataAnnotations;

public class EventViewModel
{
    public string Name { get; set; } = default!;
    public string? Location { get; set; }
    public string? Notes { get; set; }

    [Required]
    public string OwnerUserId { get; set; }
    public List<string> ParitipantIds { get; set; } = new();
}
