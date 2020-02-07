using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Data;

namespace Project.Service
{
    public class AdminService : IAdmin
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateModerator(string id)
        {
            var userStore = new UserStore<ApplicationUser>(_context);
            var user = _context.ApplicationUsers.Find(id);
            
            userStore.AddToRoleAsync(user, "Moderator").Wait();
         //   userStore.AddToRoleAsync(user, "moderator").Wait();

            await _context.SaveChangesAsync();
        }

        public async Task RemoveModerator(string id)
        {
            var userStore = new UserStore<ApplicationUser>(_context);
            var user = _context.ApplicationUsers.Find(id);

            userStore.RemoveFromRoleAsync(user, "Moderator").Wait();
           // userStore.RemoveFromRoleAsync(user, "moderator").Wait();

            await _context.SaveChangesAsync();
        }
    }
}