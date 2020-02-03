using System.Threading.Tasks;
using Project.Data;
using StopGambleProject.Data;

namespace Project.Service
{
    public class AdminService : IAdmin
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Task CreateModerator(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}