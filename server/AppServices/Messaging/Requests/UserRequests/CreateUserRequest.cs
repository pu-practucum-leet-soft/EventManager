namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserRequest : ServiceRequestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public CreateUserRequest(UserModel user)
        {
            User = user;
        }
    }
}
