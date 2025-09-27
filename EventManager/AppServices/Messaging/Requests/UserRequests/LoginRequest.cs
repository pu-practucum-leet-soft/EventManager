namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    public class LoginRequest : ServiceRequestBase
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
