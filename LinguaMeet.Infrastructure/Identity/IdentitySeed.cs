using LinguaMeet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace LinguaMeet.Infrastructure.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1️⃣ Création des rôles
            string[] roles = { "User", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine($"Erreur création rôle {role} :");
                        foreach (var error in roleResult.Errors)
                            Console.WriteLine(error.Description);
                    }
                }
            }

            // 2️⃣ Création de l'admin par défaut
            var adminEmail = "admin@linguameet.com";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin LinguaMeet",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (!result.Succeeded)
                {
                    Console.WriteLine("Erreur création admin :");
                    foreach (var error in result.Errors)
                        Console.WriteLine(error.Description);
                }
                else
                {
                    var roleAddResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!roleAddResult.Succeeded)
                    {
                        Console.WriteLine("Erreur attribution rôle Admin :");
                        foreach (var error in roleAddResult.Errors)
                            Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}
