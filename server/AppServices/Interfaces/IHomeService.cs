using EventManager.AppServices.Messaging.Responses.HomeResponses;

namespace EventManager.AppServices.Interfaces;

/// <summary>
/// Декларира функционалността за работа със съдържанието на началната страница.
/// </summary>
public interface IHomeService
{
    /// <summary>
    /// Зарежда информацията за началната страница на потребителя, включително предстоящи събития,
    /// скорошни събития и покани.
    /// </summary>
    /// <param name="userId">Уникален идентификатор на потребителя, за когото се зареждат данните.</param>
    /// <returns><see cref="GetHomeResponse"/> със списък от предстоящи събития, скорошни събития и покани.</returns>
    Task<GetHomeResponse> GetHome(Guid userId);
}