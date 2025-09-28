namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateJwtResponse : ServiceResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TokenExpiryTime { get; set; }
    }
}
