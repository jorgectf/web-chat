using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebChat.Application.API.Common.Identity;
using WebChat.Application.API.Common.Interfaces;
using WebChat.Infrastructure.API.Identity.Common.Extensions;
using WebChat.Infrastructure.API.Identity.Common.Models;
using WebChat.Infrastructure.API.Identity.Common.Server;
using IdentityResult = WebChat.Application.API.Common.Identity.IdentityResult;

namespace WebChat.Infrastructure.API.Identity.Services
{
    public class IdentityService : AbstractIdentityServer, IIdentityService
    {
        private readonly UserManager<ApplicationUser> _manager;

        public IdentityService(UserManager<ApplicationUser> manager, IOptions<IdentitySettings> settings) :
            base(settings)
        {
            _manager = manager;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await GetUserAsync(userId);
            
            return user.UserName;
        }

        public async Task<(IdentityResult Result, string Token)> GetUserAsync(string userName, string password)
        {
            var user=await _manager.FindByNameAsync(userName);
            var result = await _manager.CheckPasswordAsync(user, password);

            return (IdentityResult.Success(), CreateToken(user));
        }

        public async Task<bool> UserIsInRole(string userId, string role)
        {
            var user = await GetUserAsync(userId);

            return await _manager.IsInRoleAsync(user, role);
        }

        public async Task<(IdentityResult Result, string Token)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser {UserName = userName, Email = userName};
            var result = await _manager.CreateAsync(user, password);

            return (result.ToApplicationResult(), CreateToken(user));
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await GetUserAsync(userId);

            if (user != null) return await DeleteUserAsync(user);

            return IdentityResult.Success();
        }

        // Helpers.

        private async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await _manager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        private async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _manager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}