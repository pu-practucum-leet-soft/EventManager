using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventManager.AppServices.Interfaces
{
    public interface IJwtHelper
    {

         Task<string> GenerateJwtToken(string userId, string role);
    }
}
