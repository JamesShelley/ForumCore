using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Data.Models;

namespace StopGambleProject.Controllers
{
    public class PostController : Controller
    {
       
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public PostController(IPost postService)
        {
            postService = _postService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);


            return View();
        }
    }
}