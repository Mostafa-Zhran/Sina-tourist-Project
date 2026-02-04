using Microsoft.AspNetCore.Identity;
using Sina_DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Repository.Authintcation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly SinaDbContext _AppContext;
        private readonly UserManager<ApplicationUser> _UserManager;

        public AuthenticationRepository(SinaDbContext appContext, UserManager<ApplicationUser> userManager)
        {
            _AppContext = appContext;
            _UserManager = userManager;
        }

        public async Task<(bool IsSuccess, string? Error)> ChangePassword(ApplicationUser user, string currentPassword, string newPassword)
        {
            var signInResult = await _UserManager.CheckPasswordAsync(user, currentPassword);
            if (!signInResult)
            {
                return (false, "Current password is incorrect.");
            }

            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                return (false, "New password is too weak. It should be at least 6 characters long.");
            }

            var result = await _UserManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            return (true, null);
        }

        public async Task<(bool IsSuccess, string? Error)> CheckPassword(ApplicationUser user, string Passwors)
        {
            bool validPassword = await _UserManager.CheckPasswordAsync(user, Passwors);
            if (!validPassword)
            {
                var errors = string.Join("; ", "Invalid Email or Password");
                return (false, errors);
            }
            return (true, null);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _UserManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<(bool IsSuccess, string? Error)> User_RegisterAsync(ApplicationUser user, string Password)
        {
            var result = await _UserManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }
            return (true, null);
        }

        public async Task<(bool IsSuccess, string? Error)> DeleteUser(ApplicationUser user)
        {
            var result = await _UserManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }
            return (true, null);

        }

        public async Task RevokeTokenAsync(string token, DateTime expiration)
        {
            var revoked = new RevokedToken
            {
                Token = token,
                ExpirationDate = expiration
            };

           // await _AppContext.RevokedTokens.AddAsync(revoked);
            await _AppContext.SaveChangesAsync();
        }
    }
}
