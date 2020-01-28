using Microsoft.AspNetCore.Mvc;
using Project.Data;
using StopGambleProject.Models.Forum;
using System.Linq;

namespace StopGambleProject.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel
                {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
                });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id)
        {
            var forums = _forumService.GetById(id);
            var posts = _postService.GetFilteredPosts(id);
            

            return View();
        }
    }
}