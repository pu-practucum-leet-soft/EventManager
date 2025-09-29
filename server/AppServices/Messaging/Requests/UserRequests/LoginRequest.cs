namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginRequest : ServiceRequestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
