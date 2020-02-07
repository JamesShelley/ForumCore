using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Data.Models;

namespace Project.Data
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        IEnumerable<ApplicationUser> GetActiveUsers(int id);
        bool HasRecentPost(int id);
        Task Create(Forum forum);
        Task Delete(int forumId);
        Task EditForum(int forumId, string newTitle, string newDescription);
    }
}
