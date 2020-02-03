using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data.Models;
using StopGambleProject.Data;

namespace Project.Data
{
    public class DataSeeder
    {
        private ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<ApplicationUser>(_context);
     
            //When converting to SaaS - change this to user values
            var user = new ApplicationUser
            {
                UserName = "Admin",
                NormalizedUserName = "admin",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var hasher = new PasswordHasher<ApplicationUser>();
            //When converting to SaaS - change this to user values
            var hashedPassword = hasher.HashPassword(user, "admin");
            user.PasswordHash = hashedPassword;
            
            var hasAdminRole = _context.Roles.Any(roles => roles.Name == "Admin");
            var hasModeratorRole = _context.Roles.Any(roles => roles.Name == "Moderator");

            if (!hasAdminRole)
            { 
                roleStore.CreateAsync(new IdentityRole {Name = "Admin", NormalizedName = "admin"});
            }
            if (!hasModeratorRole)
            { 
                roleStore.CreateAsync(new IdentityRole {Name = "Moderator", NormalizedName = "moderator"});
            }

            var hasSuperUser = _context.Users.Any(u => u.UserName == user.UserName);

            if (!hasSuperUser)
            {
                 userStore.CreateAsync(user);
                 userStore.AddToRoleAsync(user, "admin");
                 userStore.AddToRoleAsync(user, "Admin");
            }

            _context.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}