using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data.Models;
using StopGambleProject.Data;
using StopGambleProject.Models.Admin;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Data;
using StopGambleProject.Models.ApplicationUser;

namespace StopGambleProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUser _userService;
        private readonly IAdmin _adminService;
        private readonly IPost _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, IPost postService, IApplicationUser userService, IAdmin adminService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _postService = postService;
            _adminService = adminService;
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PromoteModerator(string userId)
        {
            await _adminService.CreateModerator(userId);
            return RedirectToAction("ModeratorPanel", "Admin");
        }
        private bool IsUserModerator(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Moderator");
        }
        private bool IsUserAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }
        
        public IActionResult ModeratorPanel()
        {
            var profiles = _userService.GetAll().OrderBy(user => user.UserName)
                .Select(u => new ProfileModel
                {
                    Email = u.Email,
                    UserId = u.Id,
                    UserName = u.UserName,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince,
                    PostCount = _postService.GetUserPostCount(u.Id),
                    IsAdmin = IsUserAdmin(u),
                    IsModerator = IsUserModerator(u)
                });
            
            var model = new ProfileListModel
            {
                Profiles = profiles
            };

            return View(model);
        }
        
    }
}