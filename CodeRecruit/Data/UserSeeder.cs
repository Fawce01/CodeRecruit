using CodeRecruit.Constants;
using Microsoft.AspNetCore.Identity;

namespace CodeRecruit.Data
{
    public class UserSeeder
    {

        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserWithRole(userManager, "admin@coderecruit.com", "Admin123!", Roles.Admin);
            await CreateUserWithRole(userManager, "jobseeker@coderecruit.com", "Jobseeker123!", Roles.JobSeeker);
            await CreateUserWithRole(userManager, "employer@coderecruit.com", "Employer123!", Roles.Employer);
        }

        public static async Task CreateUserWithRole(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                // user manager has not found an account with this email
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email,
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed to create user, Erros: {string.Join(",", result.Errors)}");
                }
            }
        }
    }
}
