using System.Threading.Tasks;

namespace Project.Data
{
    public interface IAdmin
    {
        Task CreateModerator(string id);
        Task RemoveModerator(string id);
    }
}