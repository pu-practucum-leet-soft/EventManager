namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    public class CreateJwtResponse : ServiceResponseBase
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpiryTime { get; set; }
    }
}
