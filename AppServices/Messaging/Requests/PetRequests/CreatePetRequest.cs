namespace EventManager.AppServices.Messaging.Requests.PetRequests
{
    public class CreatePetRequest : ServiceRequestBase
    {
        public PetModel Pet { get; set; }

        public CreatePetRequest(PetModel pet)
        {
            Pet = pet;
        }
    }
}
