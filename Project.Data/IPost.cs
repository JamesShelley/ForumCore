using Project.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data
{
    public interface IPost
    {
        Post GetById(int id);
        int GetUserPostCount(string id);
        int GetPostReplyCount(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery);
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPosts(int numberOfPosts);
        Task DeletePostsInForum(int forumId);
        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int id, string newContent, string newTitle);
        Task AddReply(PostReply reply);
    }
}
