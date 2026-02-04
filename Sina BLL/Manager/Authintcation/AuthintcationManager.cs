using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sina_BLL.DTO.AuthintcationDto;
using Sina_DAL.Model;
using Sina_DAL.Repository.Authintcation;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sina_BLL.Manager.Authintcation
{
   public class AuthintcationManager : IAuthenticationManager
   {
     private readonly UserManager<ApplicationUser> _userManager;
     private readonly IConfiguration _Configuration;
     private readonly IAuthenticationRepository _AuthenticationRepository;
     private readonly IMapper _Mapper;
      

      public AuthintcationManager
          (
          UserManager<ApplicationUser> userManager,
          IConfiguration configuration,
          IAuthenticationRepository authenticationRepository,
          IMapper mapper               
          )
            {
                _userManager = userManager;
                _Configuration = configuration;
                _AuthenticationRepository = authenticationRepository;
                _Mapper = mapper;
            }

      private async Task<AuthResponseDto> CreateAuthResponseSuccess(ApplicationUser user, string message)
            {
                var token = await GenerateToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                var Response = _Mapper.Map<AuthResponseDto>(user);

                Response.IsSuccess = true;
                Response.Message = message;
                Response.Token = token;
                Response.Roles = roles.ToList();

                return Response;

            }
      private async Task<string> GenerateToken(ApplicationUser user)
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var jwtSection = _Configuration.GetSection("JWT");

                var securityKey = jwtSection["SecurityKey"];
                var issuer = jwtSection["Issuer"];
                var audience = jwtSection["Audience"];
                var duration = jwtSection["DurationtimeInMinutes"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(duration)),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
      public async Task<AuthResponseDto> RegisterUserAsync(UserRegisterDto userRegister)
            {
                if (userRegister == null)
                    throw new ArgumentNullException(nameof(userRegister));

                var existUser = await _userManager.FindByEmailAsync(userRegister.Email);
                if (existUser != null)
                    throw new Exception("User Already Exists");

                if (userRegister.Password != userRegister.ConfirmPassword)
                    return new AuthResponseDto { IsSuccess = false, Message = "Passwords mismatch" };

                var user = _Mapper.Map<ApplicationUser>(userRegister);
                user.UserName = userRegister.Email;


                var result = await _AuthenticationRepository.User_RegisterAsync(user, userRegister.Password);

                if (result.IsSuccess)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return await CreateAuthResponseSuccess(user, "User Registered Successfully");
                }
                return await CreateAuthResponseFailed(user, result.Error); ;

            }
      public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
            {
                if (loginDto == null)
                    throw new ArgumentNullException(nameof(loginDto));

                var IsExist = await _AuthenticationRepository.GetUserByEmailAsync(loginDto.Email);
                if (IsExist == null)
                {
                    return await CreateAuthResponseFailed(IsExist, "\"Invalid Email or Password\"");
                }

                var IsVaild = await _AuthenticationRepository.CheckPassword(IsExist, loginDto.Password);
                if (!IsVaild.IsSuccess)
                {
                    return await CreateAuthResponseFailed(IsExist, IsVaild.Error);
                }
                return await CreateAuthResponseSuccess(IsExist, "User Registered Successfully");
            }
      public async Task<AuthResponseDto> RegisterAdminAsync(AdminRegisterDto userRegister)
            {
                if (userRegister == null)
                    throw new ArgumentNullException(nameof(userRegister));

                var existUser = await _userManager.FindByEmailAsync(userRegister.Email);
                if (existUser != null)
                    throw new Exception("User Already Exists");

                if (userRegister.Password != userRegister.ConfirmPassword)
                    return new AuthResponseDto { IsSuccess = false, Message = "Passwords mismatch" };

                var user = _Mapper.Map<ApplicationUser>(userRegister);
                user.UserName = userRegister.Email;


                var result = await _AuthenticationRepository.User_RegisterAsync(user, userRegister.Password);

                if (result.IsSuccess)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");

                    return await CreateAuthResponseSuccess(user, "User Registered Successfully");
                }
                return await CreateAuthResponseFailed(user, result.Error); ;

            }
      private async Task<AuthResponseDto> CreateAuthResponseFailed(ApplicationUser user, string message)
            {
                if (user != null)
                {
                    var Response = _Mapper.Map<AuthResponseDto>(user);
                    Response.IsSuccess = false;
                    Response.Message = message;

                    return Response;
                }

                var response = new AuthResponseDto
                {
                    Id = null,
                    Email = null,
                    Roles = null,
                    Token = null,
                    IsSuccess = false,
                    Message = message
                };
                return response;
            }
      public async Task<AuthResponseDto> ChangePasswordAsync(string Id, ChangePasswordDto changePassword)
            {
                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                    return await CreateAuthResponseFailed(null, "User not found");

                if (changePassword.CurrentPassword == changePassword.NewPassword)
                    return await CreateAuthResponseFailed(user, "New password cannot be the same as the current password");

                if (changePassword.ConfirmPassword != changePassword.NewPassword)
                    return await CreateAuthResponseFailed(user, "Passwords mismatch");

                var result = await _AuthenticationRepository.ChangePassword(
                    user,
                    changePassword.CurrentPassword,
                    changePassword.NewPassword
                );

                if (!result.IsSuccess)
                    return await CreateAuthResponseFailed(user, result.Error);

                return await CreateAuthResponseSuccess(user, "Password changed successfully");
            }
      public async Task<AuthResponseDto> DeleteUserAsync(string id)
            {
                if (string.IsNullOrEmpty(id))
                    return await CreateAuthResponseFailed(null, "Invalid user id");

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return await CreateAuthResponseFailed(null, "User not found");

                var response = await _AuthenticationRepository.DeleteUser(user);

                if (!response.IsSuccess)
                    return await CreateAuthResponseFailed(user, response.Error);

                return await CreateAuthResponseSuccess(user, "User deleted Successfully");
            }
      public async Task<AuthResponseDto> LogoutAsync(string token)
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Token is missing."
                    };
                }


                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var expiration = jwtToken.ValidTo;

                await _AuthenticationRepository.RevokeTokenAsync(token, expiration);

                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Logged out successfully."
                };
            }
   }
}

