namespace EventManager.AppServices.Messaging
{
    public class ServiceResponseBase
    {
        public BusinessStatusCodeEnum StatusCode { get; set; }
       
        public string Message { get; set; }

        public ServiceResponseBase()
        {
            StatusCode = BusinessStatusCodeEnum.None;
        }

        public ServiceResponseBase(BusinessStatusCodeEnum statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
