using EventManager.AppServices.Messaging.Responses.HomeResponses;

namespace EventManager.AppServices.Interfaces;

public interface IHomeService
{
    Task<GetHomeResponse> GetHome(Guid userId);
}