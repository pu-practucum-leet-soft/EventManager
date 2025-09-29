namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenResponse : ServiceResponseBase
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
        /// /
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TokenExpiryTime { get; set; }
    }
}
