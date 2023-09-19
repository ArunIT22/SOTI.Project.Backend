using SOTI.Project.DAL.Models;
using System.Threading.Tasks;

namespace SOTI.Project.DAL.Interfaces
{
    public interface IAccount
    {
        Task<User> ValidateUser(string username, string password);
    }
}
