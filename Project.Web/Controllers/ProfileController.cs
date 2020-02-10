using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Data;
using Project.Data.Models;
using ReflectionIT.Mvc.Paging;
using StopGambleProject.Data;
using StopGambleProject.Models.ApplicationUser;
using StopGambleProject.Models.Post;

namespace StopGambleProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        private readonly IPost _postService;
        private readonly ApplicationDbContext _context;


        public  ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload upload, IConfiguration configuration, IPost postService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = upload;
            _configuration = configuration;
            _postService = postService;
            _context = context;

        }
        
        public async Task<IActionResult> Index(int page = 1)
        {
            if(User.Identity.IsAuthenticated)
            {
                var profiles = _userService.GetAll().OrderBy(user => user.MemberSince)
                 .Select(u => new ProfileModel
                 {
                     Email = u.Email,
                     UserId = u.Id,
                     UserName = u.UserName,
                     ProfileImageUrl = u.ProfileImageUrl,
                     UserRating = u.Rating.ToString(),
                     MemberSince = u.MemberSince,
                     PostCount = _postService.GetUserPostCount(u.Id)
                 });
                
                var model = new ProfileListModel
                {
                    Profiles = profiles
                };
                
                //TODO refractor this View Model instead of Data Model
               var item = _context.ApplicationUsers.AsNoTracking().OrderBy(user => user.MemberSince);
              var dataModel = await PagingList<ApplicationUser>.CreateAsync(item, 4, page);
               return View(dataModel);

            } else
            {
                return Redirect("/Identity/Account/Register");
            }

        }

        public IActionResult Detail(string id)
        {
            if(id == null)
            {
                return Redirect("/");
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = _userService.GetById(id);
                var postCount = _postService.GetUserPostCount(user.Id);
                var userRoles = _userManager.GetRolesAsync(user).Result;
                
                var latestPosts = _postService.GetLatestPosts(5);
                var usersPosts = _userService.GetUserPosts(id, 5);
                var posts = usersPosts.Select(post => new PostListingModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    AuthorName = post.User.UserName,
                    AuthorId = post.User.Id,
                    AuthorRating = post.User.Rating,
                    DatePosted = post.Created.ToString(),
                    RepliesCount = post.Replies.Count(),
                });
                
                var model = new ProfileModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserRating = user.Rating.ToString(),
                    Email = user.Email,
                    ProfileImageUrl = user.ProfileImageUrl,
                    MemberSince = user.MemberSince.Date,
                    IsAdmin = userRoles.Contains("Admin"),
                    IsModerator = userRoles.Contains("Moderator"),
                    PostCount = postCount,
                    UserPosts = posts
                    
                };
                return View(model);
            } else
            {
                return Redirect("/Identity/Account/Register");
            }
        }
        public IActionResult LockoutUser(string id)
        {
            id = _userManager.GetUserId(User);
            _userService.DeactivateUser(id);

            return RedirectToAction("Detail", "Profile");
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);

            //Connect to an azure storage account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");

            //Get blob container
            var container = _uploadService.GetBlobContainer(connectionString, "profile-images");

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

        [HttpPost]
        public async Task<IActionResult> DeleteProfileImage(string userId)
        {
            await _userService.DeleteProfileImage(userId);

            return RedirectToAction("Detail", "Profile", new {id = userId});
        }

    }
}