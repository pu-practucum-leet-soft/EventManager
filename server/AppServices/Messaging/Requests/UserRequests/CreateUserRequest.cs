namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// Заявка за създаване на нов потребител в системата.
    /// </summary>
    public class CreateUserRequest : ServiceRequestBase
    {
        /// <summary>
        /// Данни за новия потребител, включително потребителско име, имейл и парола.
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Създава нова инстанция на <see cref="CreateUserRequest"/> с подадения потребител.
        /// </summary>
        /// <param name="user">Модел с информация за новия потребител.</param>
        public CreateUserRequest(UserModel user)
        {
            User = user;
        }
    }
}
