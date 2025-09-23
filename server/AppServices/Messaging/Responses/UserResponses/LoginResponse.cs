namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    public class LoginResponse : ServiceResponseBase
    {
        public string Message { get; set; }

        public string FristName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }
        public Guid Id { get; set; } 
    }
}
