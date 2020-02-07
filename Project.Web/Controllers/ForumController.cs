using System;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Models.Forum;
using StopGambleProject.Models.Post;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Blob;
using Project.Web;
using ReflectionIT.Mvc.Paging;

namespace StopGambleProject.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        
        public ForumController(IForum forumService, IPost postService, IUpload upload, IConfiguration configuration)
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = upload;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel
                {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description,
                    ImageUrl = forum.ImageUrl,
                    NumberOfPosts = forum.Posts?.Count() ?? 0,
                    NumberOfUsers = _forumService.GetActiveUsers(forum.Id).Count(),
                    HasRecentPost = _forumService.HasRecentPost(forum.Id)
                    
                });

            var model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(f => f.Name)
            };

            return View(model);
        }

        public  IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();
        
            var postListings = posts.Select(post => new PostListingModel {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            }).OrderByDescending(post => post.DatePosted);
            
            var model = new ForumTopicModel
            {
                Posts = postListings, 
                Forum = BuildForumListing(forum)
            };
            return View(model);
        }

        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                var model = new AddForumModel();
                return View(model);
            } else
            {
                return Redirect("/");
            }
        }
        
        public async Task<IActionResult> Edit(ForumTopicModel model)
        {
            await _forumService.EditForum(model.Forum.Id,model.Forum.Name, model.Forum.Description);
            return RedirectToAction("Topic","Forum", new { id = model.Forum.Id });
        }
        
        public IActionResult DeleteForum(Forum forum)
        {
            _forumService.Delete(forum.Id).Wait();
            return RedirectToAction("Index", "Forum");
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/forum/default.png";
            
            if(model.ImageUpload != null)
            {
                var blockBlob = UploadForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }
            
            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = imageUri,
                Created = DateTime.Now
            };

            await _forumService.Create(forum);
            return RedirectToAction("Index", "Forum");
        }

        private CloudBlockBlob UploadForumImage(IFormFile file)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");

            var container = _uploadService.GetBlobContainer(connectionString, "forum-images");

            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var fileName = contentDisposition.FileName.Trim('"');

            var blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.UploadFromStreamAsync(file.OpenReadStream()).Wait();

            return blockBlob;
        }

        // id = forum id, searchQuery = users search
        [HttpPost]
        public IActionResult Search(int id, string searchQuery) 
        {
            return RedirectToAction("Topic", new {id, searchQuery});
        }
        
        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
        
       
    }
}