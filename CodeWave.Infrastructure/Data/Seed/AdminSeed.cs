using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Data.Seed
{
    public static class AdminSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Check if admin user already exists
            var adminEmail = "admin@codewave.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            
            if (existingAdmin != null)
            {
                // Update existing admin if needed
                if (!existingAdmin.IsAdmin)
                {
                    existingAdmin.IsAdmin = true;
                    await userManager.UpdateAsync(existingAdmin);
                }
                return;
            }

            // Create admin user
            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                IsAdmin = true,
                Level = "Advanced",
                LearningPath = "Admin"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            
            if (result.Succeeded)
            {
                // Add admin claim
                await userManager.AddClaimAsync(adminUser, new System.Security.Claims.Claim("IsAdmin", "true"));
                Console.WriteLine("Admin user created successfully: admin@codewave.com / Admin@123");
            }
            else
            {
                Console.WriteLine($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}

