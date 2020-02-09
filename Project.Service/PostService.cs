using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Data.Models;
using StopGambleProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Project.Service
{
    public class PostService : IPost
    {

        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }
        
        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = GetById(id);
            var repliesToPost = _context.PostReplies.Where(postReply => postReply.Post.Id == id);

            /*
            * Delete Replies
            */
            foreach (var postReply in repliesToPost)
            {
                _context.PostReplies.Remove(postReply);
            }

            _context.Remove(post);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeletePostsInForum(int forumId)
        {
            var forum = GetPostsByForum(forumId);
            var postsInForum = _context.Posts.Where(post => post.Forum.Id == forumId);
            var repliesInForum = _context.PostReplies.Where(postReply => postReply.Post.Forum.Id == forumId);

            /*
             * Delete Replies
             */
            foreach (var postReply in repliesInForum)
            {
                _context.PostReplies.Remove(postReply);
            }

            /*
            * Delete Posts
            */
            foreach (var post in postsInForum)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditPostContent(int id, string newContent, string newTitle)
        {
            var post = GetById(id);
            post.Content = newContent;
            post.Title = newTitle;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.Include(post => post.User)
                 .Include(post => post.Replies).ThenInclude(reply => reply.User)
                 .Include(post => post.Forum);
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                 .Include(post => post.User)
                 .Include(post => post.Replies).ThenInclude(reply => reply.User)
                 .Include(post => post.Forum)
                 .First();
        }

        public int GetPostReplyCount(int id)
        {
            return _context.PostReplies.Where(postReply => postReply.Post.Id == id).Count();
        }

        public int GetUserPostCount(string id)
        {
            return _context.Posts.Where(post => post.User.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .Count();
        }
        
        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery) ? forum.Posts : forum.Posts.Where
                (post => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            return GetAll().Where(post => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetLatestPosts(int numberOfPosts)
        {
            return GetAll().OrderByDescending(post => post.Created).Take(numberOfPosts);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return _context.Forums.Where(forum => forum.Id == id).First().Posts;
        }
    }
}
