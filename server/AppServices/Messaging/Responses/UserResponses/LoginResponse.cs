namespace EventManager.AppServices.Messaging.Responses.UserResponses
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginResponse : ServiceResponseBase
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
        public string? UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Role { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TokenExpiryTime { get; set; }
    }
}
