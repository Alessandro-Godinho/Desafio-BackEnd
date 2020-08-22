using System.Threading.Tasks;
using DesafioBackEnd.Models;

namespace DesafioBackEnd.Data
{
    public interface IAuthRepository
    {
         Task<User>Register(User user, string password);
         Task<User>Login(string username, string password);
         Task<bool>IsUserExists(string username);

    }
}