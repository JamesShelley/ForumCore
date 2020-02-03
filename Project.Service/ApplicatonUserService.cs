using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Data;

namespace Project.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public ApplicationUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public Task UpdateUserRating(string id, Type type)
        {
            var user = GetById(id);
            user.Rating += CalculateUserRating(type, user.Rating);
            return _context.SaveChangesAsync();
        }

        public Task DeactivateUser(string id)
        {
            var user = GetById(id);
            user.LockoutEnd = DateTimeOffset.Now .AddYears(1);
            return _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int rating)
        {
            var inc = 0;
            if (type == typeof(Post))
            {
                inc = 2;
            }

            if (type == typeof(PostReply))
            {
                inc = 1;
            }

            return inc;
        }
    }
}