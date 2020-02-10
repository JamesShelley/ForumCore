using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Data.Models;

namespace Project.Data
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        IOrderedQueryable<Forum> PaginateTopics();
        IEnumerable<ApplicationUser> GetActiveUsers(int id);
        bool HasRecentPost(int id);
        Task Create(Forum forum);
        Task Delete(int forumId);
        Task EditForum(int forumId, string newTitle, string newDescription);
    }
}
