using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Models.ApplicationUser;

namespace StopGambleProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        
        public  ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload upload, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = upload;
            _configuration = configuration;
        }

        // GET
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;
            
            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin")
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);

            //Connect to an azure storage account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");

            //Get blob container
            var container = _uploadService.GetBlobContainer(connectionString);

            //Parse the content disposition response header 
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            //Grab the filename
            var fileName = contentDisposition.FileName.Trim('"');

            //Get a reference to the block blob.
            var blockBlob = container.GetBlockBlobReference(fileName);

            //On that block blob, upload the file <- file has been uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            //Set the user profile image to the Uri that is returned from the block blob. 
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            //Redirect to users profile page.
            return RedirectToAction("Detail", "Profile", new { id = userId });
            
        }
    }
}