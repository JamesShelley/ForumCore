using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StopGambleProject.Models.Post;

namespace StopGambleProject.Models.ApplicationUser
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserRating { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public DateTime MemberSince { get; set; }
        public IFormFile ImageUpload { get; set; }
        public int PostCount { get; set; }
        
        public IEnumerable<PostListingModel> UserPosts { get; set; }
    }
}