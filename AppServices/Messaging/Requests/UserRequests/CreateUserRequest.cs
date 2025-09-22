namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    public class CreateUserRequest : ServiceRequestBase
    {
        public UserModel User { get; set; }

        public CreateUserRequest(UserModel user)
        {
            User = user;
        }
    }
}
