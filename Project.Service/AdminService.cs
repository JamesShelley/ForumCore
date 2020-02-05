using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Data;

namespace Project.Service
{
    public class AdminService : IAdmin
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUserService _userService;

        public AdminService(ApplicationDbContext context, ApplicationUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        
        public async Task CreateModerator(string id)
        {
            var userStore = new UserStore<ApplicationUser>(_context);
            var user = _userService.GetById(id);          
            
            userStore.AddToRoleAsync(user, "Moderator").Wait();
            userStore.AddToRoleAsync(user, "moderator").Wait();
            
            await _context.SaveChangesAsync();
        }
    }
}