using EventManager.AppServices.Messaging.Responses.UserResponses;
using EventManager.Data.Entities;
using EventManager.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.AppServices.Messaging.Responses.EventResponses;

public class EventViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;


    public string? Description { get; set; }


    public DateTime? StartDate { get; set; }

    public string? Location { get; set; }


    public Guid OwnerUserId { get; set; }

    public EventStatus Status { get; set; }

    public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
}


