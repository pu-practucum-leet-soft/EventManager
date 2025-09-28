namespace EventManager.AppServices.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        public BusinessStatusCodeEnum StatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ServiceResponseBase()
        {
            StatusCode = BusinessStatusCodeEnum.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        public ServiceResponseBase(BusinessStatusCodeEnum statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
