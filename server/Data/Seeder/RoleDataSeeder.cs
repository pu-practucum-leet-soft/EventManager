namespace EventManager.Data.Seeder
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Отговаря за първоначалното създаване на роли в системата.
    /// Гарантира, че основните роли като "Admin" и "User"
    /// ще бъдат налични при стартиране на приложението.
    /// </summary>
    public class RoleDataSeeder
    {
        /// <summary>
        /// Проверява дали дефинираните роли съществуват в базата данни
        /// и ги създава, ако липсват.
        /// </summary>
        /// <param name="roleManager">
        /// Инстанция на <see cref="RoleManager{T}"/> за работа с роли в ASP.NET Identity.
        /// </param>
        /// <returns>
        /// Задача, която завършва, когато създаването на роли приключи.
        /// </returns>
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }
    }
}
