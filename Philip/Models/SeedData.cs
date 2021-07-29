using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Philip.Data;

namespace Philip.Models
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            using var context = new PhilipContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PhilipContext>>());
            // Look for any movies.
            if (context.Article.Any())
            {
                // DB has been seeded
            }
            else
            {
                context.Article.AddRange(
                new Article
                {
                    Title = "SQL Injection",
                    Author = "Nicolas Teo",
                    ReleaseDate = DateTime.Parse("2021-06-27"),
                    Content = "SQL injection is a web security vulnerability that allows an attacker to interfere with the queries that an application makes to its database. It generally allows an attacker to view data that they are not normally able to retrieve. This might include data belonging to other users, or any other data that the application itself is able to access. In many cases, an attacker can modify or delete this data, causing persistent changes to the application's content or behavior. In some situations, an attacker can escalate an SQL injection attack to compromise the underlying server or other back-end infrastructure, or perform a denial-of-service attack."
                },

                new Article
                {
                    Title = "Cross Site Scripting ",
                    Author = "Nicholas Boey",
                    ReleaseDate = DateTime.Parse("2021-05-23"),
                    Content = "Cross-Site Scripting (XSS) attacks are a type of injection, in which malicious scripts are injected into otherwise benign and trusted websites. XSS attacks occur when an attacker uses a web application to send malicious code, generally in the form of a browser side script, to a different end user. Flaws that allow these attacks to succeed are quite widespread and occur anywhere a web application uses input from a user within the output it generates without validating or encoding it. An attacker can use XSS to send a malicious script to an unsuspecting user.The end user’s browser has no way to know that the script should not be trusted, and will execute the script.Because it thinks the script came from a trusted source, the malicious script can access any cookies, session tokens, or other sensitive information retained by the browser and used with that site.These scripts can even rewrite the content of the HTML page."
                },

                new Article
                {
                    Title = "Command injection ",
                    Author = "Dexter Low",
                    ReleaseDate = DateTime.Parse("2021-04-21"),
                    Content = "Command injection is a cyber attack that involves executing arbitrary commands on a host operating system (OS). Typically, the threat actor injects the commands by exploiting an application vulnerability, such as insufficient input validation."
                },

                new Article
                {
                    Title = "Hashing",
                    Author = "Nicholas Chng",
                    ReleaseDate = DateTime.Parse("2021-03-18"),
                    Content = "Hashing is the process of transforming any given key or a string of characters into another value. This is usually represented by a shorter, fixed-length value or key that represents and makes it easier to find or employ the original string."
                }
            );
            }

            if (!context.Roles.Any())
            {
                await SeedRolesAsync(userManager, roleManager);
                // Roles dont exist
            }
            if (!context.Users.Any())
            {
                await SeedSuperAdminAsync(userManager, roleManager);
                // Users dont exist
            }
            context.SaveChanges();
        }


        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //Seed Roles
            ApplicationRole admin = new ApplicationRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                CreatedDate = DateTime.UtcNow,
                Description = "Site Administrator"
            };

            ApplicationRole member = new ApplicationRole
            {
                Name = "Member",
                NormalizedName = "MEMBER",
                CreatedDate = DateTime.UtcNow,
                Description = "Site Member"
            };

            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(member);
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                FullName = "Mukesh Murugan",
                EmailConfirmed = true,
                BirthDate = DateTime.UtcNow,
                Age = 0,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin123");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Member");
                }
            }
        }
    }
}
