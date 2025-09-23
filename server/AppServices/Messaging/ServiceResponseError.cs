using System.Text.Json.Serialization;

namespace EventManager.AppServices.Messaging
{
    public class ServiceResponseError : ServiceResponseBase
    {
        [JsonIgnore]
        public string? DeveloperError { get; set; }

        public string Message { get; set; }

        public ServiceResponseError() : base(BusinessStatusCodeEnum.InternalServerError) { }
    }
}
