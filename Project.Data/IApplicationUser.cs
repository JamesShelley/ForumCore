using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Data.Models;

namespace Project.Data
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
        IEnumerable<Post> GetUserPosts(string id);
        IOrderedQueryable<ApplicationUser> PaginateUsers();
        IEnumerable<Post> GetUserPosts(string id, int postCount);

        Task SetProfileImage(string id, Uri uri);

        Task DeleteProfileImage(string id);
        Task UpdateUserRating(string id, Type type);

        Task DeactivateUser(string id);
    }
}