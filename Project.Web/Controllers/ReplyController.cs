using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Models.Post;
using StopGambleProject.Models.Reply;

namespace StopGambleProject.Controllers
{
    public class ReplyController : Controller
    {

        private readonly IPost _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;

        public ReplyController(IPost postService, UserManager<ApplicationUser> userManager,IApplicationUser userService)
        {
            _postService = postService;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<IActionResult> Create(int id)
        {
            //get the post to reply to 
            var post = _postService.GetById(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                //Original post
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,
                PostAuthorName = post.User.UserName,
                PostAuthorRating = post.User.Rating,
                PostAuthorId = post.User.Id,
                PostAuthorImageUrl = post.User.ProfileImageUrl,
                PostCreated = post.Created,

                //Reply 
                AuthorName = User.Identity.Name,
                AuthorImageUrl = user.ProfileImageUrl,
                AuthorId = user.Id,
                AuthorRating = user.Rating,
                IsAdmin = User.IsInRole("Admin"),

                //Forum details
                ForumName = post.Forum.Title,
                ForumId = post.Forum.Id,
                ForumImageUrl = post.Forum.ImageUrl,

                Created = DateTime.Now

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var reply = BuildReply(model, user);

            await _postService.AddReply(reply);
            
            await _userService.UpdateUserRating(userId, typeof(PostReply));

            return RedirectToAction("Index", "Post", new { id = model.PostId });

        }

        private PostReply BuildReply(PostReplyModel model, ApplicationUser user)
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }
    }
}