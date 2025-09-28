using System.Text.Json.Serialization;

namespace EventManager.AppServices.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceResponseError : ServiceResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ServiceResponseError() : base(BusinessStatusCodeEnum.InternalServerError) { }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string? DeveloperError { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Message { get; set; }
    }
}
