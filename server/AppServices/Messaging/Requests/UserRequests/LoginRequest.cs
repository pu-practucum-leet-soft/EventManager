namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// Заявка за вход (автентикация) на потребител.
    /// Съдържа необходимите креденшъли за проверка на достъпа.
    /// </summary>
    public class LoginRequest : ServiceRequestBase
    {
        /// <summary>
        /// Имейл адрес на потребителя, използван за вход.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Парола на потребителя, съответстваща на предоставения имейл.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Създава нова инстанция на <see cref="LoginRequest"/> с имейл и парола.
        /// </summary>
        /// <param name="email">Имейл адрес на потребителя.</param>
        /// <param name="password">Парола на потребителя.</param>
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
