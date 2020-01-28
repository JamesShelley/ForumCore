using System;

namespace StopGambleProject.Models.Post
{
    public class PostReplyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        public DateTime Created { get; set; }
        public string ReplyContent { get; set; }

        public int PostId { get; set; }
    }
}
