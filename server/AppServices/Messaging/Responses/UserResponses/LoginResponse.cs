namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    public class LoginResponse : ServiceResponseBase
    {
        public Guid Id { get; set; } 
        public string Message { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }

        public DateTime TokenExpiryTime { get; set; }
    }
}
