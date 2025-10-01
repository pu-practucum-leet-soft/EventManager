namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// Заявка за извличане на информация за конкретен потребител.
    /// Използва се като базова заявка, когато трябва да се подадат
    /// параметри за търсене на потребител (например по ID или имейл).
    /// </summary>
    public class GetUserRequest : ServiceRequestBase
    {
        /// <summary>
        /// Създава нова инстанция на <see cref="GetUserRequest"/>.
        /// </summary>
        public GetUserRequest()
        {
        }
    }
}
