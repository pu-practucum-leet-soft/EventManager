namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    using System.ComponentModel.DataAnnotations;

    public class StatisticViewModel
    {
        [Required]
        public Guid OwnerId{ get; set; }

        [Required]
        public int OwnerEventsCount { get; set; }

        public List<EventStatistic> EventStatistics { get; set; } = new List<EventStatistic>();
    }

    public class EventStatistic
    {
        [Required]
        public Guid EventId { get; set; }

        public EventViewModel Envent { get; set; }  
        [Required]
        public int AcceptedInvitesCount { get; set; }

        // Count of all participants with invites - Accepted, Invited, Declined
        [Required]
        public int TotalInvitedCount { get; set; }
        public double CalculatedStatsticResult { get; set; }
    }
}
