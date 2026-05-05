using DesignAutomation.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DesignAutomation.API.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var db = sp.GetRequiredService<AppDbContext>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var config = sp.GetRequiredService<IConfiguration>();
        var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        await db.Database.MigrateAsync();

        var roles = config.GetSection("Seed:Roles").Get<string[]>() ?? new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                    logger.LogError("Failed to seed role {Role}: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        var admin = config.GetSection("Seed:Admin");
        var email = admin["Email"];
        var password = admin["Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Seed:Admin missing Email or Password — skipping admin seed.");
            return;
        }

        var existing = await userManager.FindByEmailAsync(email);
        if (existing is null)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = admin["UserName"] ?? email,
                FullName = admin["FullName"],
                EmailConfirmed = true,
            };
            var create = await userManager.CreateAsync(user, password);
            if (!create.Succeeded)
            {
                logger.LogError("Failed to seed admin user: {Errors}", string.Join(", ", create.Errors.Select(e => e.Description)));
                return;
            }
            existing = user;
        }

        if (!await userManager.IsInRoleAsync(existing, "Admin"))
        {
            var addRole = await userManager.AddToRoleAsync(existing, "Admin");
            if (!addRole.Succeeded)
                logger.LogError("Failed to assign Admin role: {Errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
        }
    }
}
