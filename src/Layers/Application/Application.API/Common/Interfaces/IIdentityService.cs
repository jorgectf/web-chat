using System.Threading.Tasks;
using WebChat.Application.API.Common.Models;

namespace WebChat.Application.API.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> UserIsInRole(string userId, string role);

        Task<(IdenitityResult Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<IdenitityResult> DeleteUserAsync(string userId);
    }
}