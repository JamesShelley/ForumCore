using System;

namespace StopGambleProject.Models.Reply
{
    public class PostReplyModel
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        public DateTime Created { get; set; }
        public string ReplyContent { get; set; }
        public string ReplyTitle { get; set; }
        public bool IsAdmin { get; set; }

        // Original post details
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public DateTime PostCreated { get; set; }
        public string PostAuthorName { get; set; }
        public int PostAuthorRating { get; set; }
        public string PostAuthorId { get; set; }
        public string PostAuthorImageUrl { get; set; }

        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
        public int ForumId { get; set; }

    }
}
