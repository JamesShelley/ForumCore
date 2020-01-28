using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Models.Forum;
using StopGambleProject.Models.Post;
using System;
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
            var posts = _postService.GetPostsByForum(id);
            
            var postListings = posts.Select(post => new PostListingModel {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });
            return View();
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            throw new NotImplementedException();
        }
    }
}