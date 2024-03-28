using DentistClinic.Core.Constants;
using Microsoft.AspNetCore.Identity;
namespace Usermanagement.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedingRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Admin.ToString()});
                await roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Reception.ToString()});
                await roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.Doctor.ToString()});
                await roleManager.CreateAsync(new IdentityRole { Name = Helper.Roles.User.ToString()});
            }
        }
    }
}
