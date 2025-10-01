namespace EventManager.Data.Enums
{
    /// <summary>
    /// Представлява текущия статус на дадено събитие.
    /// Използва се за управление на жизнения цикъл на събитията в системата.
    /// </summary>
    public enum EventStatus
    {
        /// <summary>
        /// Събитието е активно и достъпно за участници.
        /// </summary>
        Active = 0,

        /// <summary>
        /// Събитието е отменено от организатора и няма да се проведе.
        /// </summary>
        Cancelled = 1,

        /// <summary>
        /// Събитието е приключило и е архивирано.
        /// </summary>
        Archived = 2
    }
}
