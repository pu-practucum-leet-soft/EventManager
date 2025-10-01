namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// Отговор при заявка за извличане на един или повече потребители.
    /// </summary>
    public class GetUserResponse
    {
        /// <summary>
        /// Колекция от намерените потребители във вид на <see cref="UserViewModel"/>.
        /// </summary>
        public List<UserViewModel>? Users { get; set; }
    }
}
