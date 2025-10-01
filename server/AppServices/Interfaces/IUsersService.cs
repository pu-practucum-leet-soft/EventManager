using System.Security.Claims;
using EventManager.AppServices.Messaging.Requests.UserRequests;
using EventManager.AppServices.Messaging.Responses.UserResponses;

namespace EventManager.AppServices.Interfaces
{
    /// <summary>
    /// Интерфейс за услугите, свързани с управлението на потребители.
    /// Съдържа бизнес логика за регистрация, автентикация, излизане от профил
    /// и управление на роли.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Създава нов потребител в системата.
        /// </summary>
        /// <param name="request">Обект със заявка за създаване на потребител.</param>
        /// <returns>Отговор със статус за резултата от създаването.</returns>
        Task<CreateUserResponse> SaveAsync(CreateUserRequest request);

        /// <summary>
        /// Връща информация за потребител по неговото Id.
        /// </summary>
        /// <param name="id">Уникален идентификатор на потребителя.</param>
        /// <returns>Модел с основни данни за потребителя или null, ако не съществува.</returns>
        Task<UserViewModel?> GetUserByIdAsync(string id);

        /// <summary>
        /// Намира идентификатора на потребител по имейл адрес.
        /// </summary>
        /// <param name="email">Имейл адрес на потребителя.</param>
        /// <returns>Обект със стойността на Id.</returns>
        Task<UserIdResponse> GetUserIdByEmailAsync(string email);

        /// <summary>
        /// Извършва автентикация на потребител чрез имейл и парола.
        /// </summary>
        /// <param name="request">Заявка за вход с имейл и парола.</param>
        /// <returns>Отговор със статус, JWT токен и данни за потребителя.</returns>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Извършва излизане от системата, като изтрива свързаните сесийни данни и токени.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Назначава роля на потребител (например "User" или "Admin").
        /// </summary>
        /// <param name="userId">Id на потребителя, който ще получи роля.</param>
        /// <param name="roleName">Името на ролята, която ще бъде назначена.</param>
        /// <returns>Текстово съобщение за успех или грешка.</returns>
        Task<string> AssignRole(string userId, string roleName);
    }
}
