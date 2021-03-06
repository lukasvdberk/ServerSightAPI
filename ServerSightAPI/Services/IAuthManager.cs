using System.Threading.Tasks;
using ServerSightAPI.Models;

namespace ServerSightAPI.Services
{
    public interface IAuthManager
    {
        Task<string> CreateToken(User user);
    }
}