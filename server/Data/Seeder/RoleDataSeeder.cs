namespace EventManager.Data.Seeder
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// 
    /// </summary>
    public class RoleDataSeeder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleManager"></param>
        /// <returns></returns>
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
