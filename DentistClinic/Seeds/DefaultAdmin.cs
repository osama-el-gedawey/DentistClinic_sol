using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using DentistClinic.Data.Context;
using Microsoft.AspNetCore.Identity;


namespace Usermanagement.Seeds
{
    public class DefaultAdmin
    {
        public static async Task SeedingAdminAsync(UserManager<ApplicationUser> userManager , ApplicationDbContext applicationDbContext)
        {

            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = "MohamedHamedy",
                Email = "doctor@gmail.com",
                PhoneNumber = "01004382527",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                
            };

            ApplicationUser isExsited = await userManager.FindByEmailAsync(applicationUser.Email);

            if (isExsited == null)
            {
                IdentityResult result = await userManager.CreateAsync(applicationUser, "Omarhatab2023$");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(applicationUser, new List<string> {Helper.Roles.Doctor.ToString() });
                }
            }
        }
    }
}
