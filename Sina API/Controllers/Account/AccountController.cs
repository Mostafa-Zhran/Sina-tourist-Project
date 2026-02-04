using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Sina_BLL.DTO.AuthintcationDto;
using Sina_BLL.Exceptions;
using Sina_BLL.Manager.Authintcation;
using System.Security.Claims;


namespace Sina_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationManager _AuthenticationManager;

        public AccountController(IAuthenticationManager authenticationManager)
        {
            _AuthenticationManager = authenticationManager;
        }

        [HttpPost("Register/User")]
        public async Task<IActionResult> RegistrationUser(UserRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                throw new BadRequestException("User Not Exist");
            }

            var Response = await _AuthenticationManager.RegisterUserAsync(userRegister);

            return Ok(Response);
        }

        [HttpPost("Register/Admin")]
        public async Task<IActionResult> RegistrationAdmin(AdminRegisterDto userRegister)
        {
            if (userRegister == null)
            {
                throw new BadRequestException("Admin Not Exist");
            }

            var Response = await _AuthenticationManager.RegisterAdminAsync(userRegister);

            return Ok(Response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            if (userLogin == null)
            {
                throw new BadRequestException(" this User Not Exist");
            }
            var Response = await _AuthenticationManager.LoginAsync(userLogin);
            return Ok(Response);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                throw new UnauthorizedException("Invalid user token");

            var result = await _AuthenticationManager.ChangePasswordAsync(userId, changePassword);

            if (!result.IsSuccess)
                throw new BadRequestException("Invalid Operation");

            return Ok(result);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            var result = await _AuthenticationManager.LogoutAsync(token);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("Delete/Profile")]
        public async Task<IActionResult> Deleteuser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Response = await _AuthenticationManager.DeleteUserAsync(userId);

            return Ok(Response);
        }
    }
}
