using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Models.Post;
using StopGambleProject.Models.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StopGambleProject.Controllers
{
    public class PostController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IApplicationUser _userService;

        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager,IApplicationUser userService)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                Content = post.Content,
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                IsAdmin = IsAuthorAdmin(post.User),
                IsModerator = IsAuthorModerator(post.User)
            };

            return View(model);
        }
        
        public IActionResult Create(int id)
        {
            var userId = _userManager.GetUserId(User);
            if(User.Identity.IsAuthenticated)
            {
                // id is forum id
                var forum = _forumService.GetById(id);

                var model = new NewPostModel
                {
                    ForumName = forum.Title,
                    ForumId = forum.Id,
                    ForumImageUrl = forum.ImageUrl,
                    AuthorName = User.Identity.Name
                };

            return View(model);
            } else
            {
                return Redirect("/");
            }
        }

        //Take info from the user in the form of the Create View Model
        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var post = BuildPost(model, user);

            await _postService.Add(post);
          
            await _userService.UpdateUserRating(userId, typeof(Post));
            
            return RedirectToAction("Index","Post", new { id = post.Id });
        }

        public async Task<IActionResult> Edit(NewPostModel model)
        {
            await _postService.EditPostContent(model.Id, model.Content, model.Title);            
            return RedirectToAction("Index","Post", new { id = model.Id });
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum
            };
        }
        
        public IActionResult DeletePost(Post post)
        {
            var forumId = post.Forum.Id;
            _postService.Delete(post.Id).Wait();
            //return Redirect("/Forum");
            return RedirectToAction("Topic", "Forum", new {id = forumId});
        }
        
        private bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }
        
        private bool IsAuthorModerator(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Moderator");
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorId = reply.User.Id,
                AuthorName = reply.User.UserName,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAdmin = IsAuthorAdmin(reply.User)
            });
        }
    }
}