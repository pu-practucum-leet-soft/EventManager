namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// 
    /// </summary>
    public class StatisticViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid OwnerId{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int OwnerEventsCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<EventStatistic> EventStatistics { get; set; } = new List<EventStatistic>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class EventStatistic
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid EventId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EventViewModel? Event { get; set; }  
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int AcceptedInvitesCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int DeclinedInvitesCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int PendingInvitesCount { get; set; }

        // Count of all participants with invites - Accepted, Invited, Declined
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int TotalInvitedCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double CalculatedStatsticResult { get; set; }
    }
}
