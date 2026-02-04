using Sina_BLL.DTO.AuthintcationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_BLL.Manager.Authintcation
{
    public interface IAuthenticationManager
    {
        Task<AuthResponseDto> RegisterUserAsync(UserRegisterDto userRegister);
        Task<AuthResponseDto> RegisterAdminAsync(AdminRegisterDto userRegister);
        Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
        Task<AuthResponseDto> ChangePasswordAsync(string Id, ChangePasswordDto changePassword);
        Task<AuthResponseDto> DeleteUserAsync(string id);

        Task<AuthResponseDto> LogoutAsync(string token);
    }
}
