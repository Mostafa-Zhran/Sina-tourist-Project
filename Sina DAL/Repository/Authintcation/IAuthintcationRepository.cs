using Sina_DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Repository.Authintcation
{
    public interface IAuthenticationRepository
    {
        Task<(bool IsSuccess, string? Error)> User_RegisterAsync(ApplicationUser user, string Password);
        Task<(bool IsSuccess, string? Error)> CheckPassword(ApplicationUser user, string Password);

        Task<ApplicationUser> GetUserByEmailAsync(string Email);
        Task<(bool IsSuccess, string? Error)> ChangePassword(ApplicationUser user, string currentPassword, string newPassword);
        Task<(bool IsSuccess, string? Error)> DeleteUser(ApplicationUser user);
        Task RevokeTokenAsync(string token, DateTime expiration);

    }
}
