using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Data.Models;

namespace StopGambleProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        
        public  ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload upload)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = upload;
        }

        // GET
        public IActionResult Index(string id)
        {
            
            return View();
        }
    }
}