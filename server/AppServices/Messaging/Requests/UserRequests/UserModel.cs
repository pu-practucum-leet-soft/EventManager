namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    public class UserModel
    {
        public string FirstName { get; set; }     
        public string LastName { get; set; }      
        public string Email { get; set; }      
        public string Password { get; set; }        
        public string? ProfileImageUrl { get; set; } 
    }
}
