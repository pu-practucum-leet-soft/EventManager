namespace EventManager.AppServices.Messaging.Responses.EventResponses
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Модел за статистика на събитията, създадени от даден собственик.
    /// </summary>
    public class StatisticViewModel
    {
        /// <summary>
        /// Идентификатор на собственика на събитията.
        /// </summary>
        [Required]
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Общ брой събития, създадени от този собственик.
        /// </summary>
        [Required]
        public int OwnerEventsCount { get; set; }

        /// <summary>
        /// Списък със статистики за всяко отделно събитие.
        /// </summary>
        public List<EventStatistic> EventStatistics { get; set; } = new List<EventStatistic>();
    }

    /// <summary>
    /// Подробна статистика за конкретно събитие.
    /// </summary>
    public class EventStatistic
    {
        /// <summary>
        /// Уникален идентификатор на събитието.
        /// </summary>
        [Required]
        public Guid EventId { get; set; }

        /// <summary>
        /// Пълният модел на събитието, към което принадлежи статистиката.
        /// </summary>
        public EventViewModel? Event { get; set; }

        /// <summary>
        /// Брой поканени участници, които са приели поканата.
        /// </summary>
        [Required]
        public int AcceptedInvitesCount { get; set; }

        /// <summary>
        /// Брой поканени участници, които са отказали поканата.
        /// </summary>
        [Required]
        public int DeclinedInvitesCount { get; set; }

        /// <summary>
        /// Брой поканени участници, които все още не са отговорили.
        /// </summary>
        [Required]
        public int PendingInvitesCount { get; set; }

        /// <summary>
        /// Общ брой поканени участници (независимо дали са приели, отказали или в изчакване).
        /// </summary>
        [Required]
        public int TotalInvitedCount { get; set; }

        /// <summary>
        /// Изчислен резултат (например процент на приетите покани спрямо всички изпратени).
        /// </summary>
        public double CalculatedStatsticResult { get; set; }
    }
}
