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
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PhilipContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PhilipContext>>()))
            {
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
                
                if (context.Roles.Any())
                {
                    // Roles already exist
                }
                else
                {

                    string[] roles = new string[] { "Admin", "Member" };

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
                    var roleStore = new RoleStore<ApplicationRole>(context);
                    roleStore.CreateAsync(admin);
                    roleStore.CreateAsync(member);
                }
                if (context.Users.Any())
                {
                    // Users already exist
                }
                else
                {
                    ApplicationUser superadmin = new ApplicationUser
                    {
                        UserName = "admin1@gmail.com",
                        Email = "admin1@gmail.com",
                        NormalizedEmail = "ADMIN1@GMAIL.COM",
                        NormalizedUserName = "ADMIN1@GMAIL.COM",
                        FullName = "Admin",
                        BirthDate = DateTime.UtcNow,
                        Age = 0,
                        EmailConfirmed = true
                    };
                    // PASSWORD SETTING?!?!??!?!?!?!
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(superadmin, "Admin123"); // Admin123 is password
                    superadmin.PasswordHash = hashed;


                    UserManager<ApplicationUser> _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
                    var userStore = new UserStore<ApplicationUser>(context);
                    var result = userStore.CreateAsync(superadmin);
                    result.Wait();
                    var result2 = _userManager.AddToRoleAsync(superadmin, "Admin");
                    result2.Wait();
                }


                context.SaveChangesAsync();
            }
        }
    }
}
