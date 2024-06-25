using AutoMapper;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PrintManagement.Application.Handle.HandleEmail;
using PrintManagement.Application.InterfaceServices;
using PrintManagement.Application.Payloads.Mappers;
using PrintManagement.Application.Payloads.RequestModels.UserRequests;
using PrintManagement.Application.Payloads.ResponseModels.DataLogin;
using PrintManagement.Application.Payloads.ResponseModels.DataRole;
using PrintManagement.Application.Payloads.ResponseModels.DataUser;
using PrintManagement.Application.Payloads.Responses;
using PrintManagement.Domain.Entities;
using PrintManagement.Domain.InterfaceRepositories.InterfaceBase;
using PrintManagement.Domain.InterfaceRepositories.InterfaceUser;
using PrintManagement.Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BcryptNet = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PrintManagement.Application.ImplementServices
{
    public class AuthService : IAuthService
    {
        private readonly IBaseReposiroty<User> _baseUserRepository;
        private readonly UserConverter _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBaseReposiroty<RefreshToken> _baseRefreshTokenRepository;
        private readonly IBaseReposiroty<ConfirmEmail> _confirmEmailRepository;
        private readonly IBaseReposiroty<Permissions> _basePermissionRepository;
        private readonly IBaseReposiroty<Role> _baseRoleRepository;
        private readonly IBaseReposiroty<Team> _baseTeamRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IBaseReposiroty<User> baseUserRepository, UserConverter mapper, IConfiguration configuration, IUserRepository<User> userRepository, IBaseReposiroty<RefreshToken> baseRefreshTokenRepository, IBaseReposiroty<ConfirmEmail> confirmEmailRepository,
            IEmailService emailService, IBaseReposiroty<Permissions> basePermissionRepository, IBaseReposiroty<Role> baseRoleRepository, IBaseReposiroty<Team> baseTeamRepository, IHttpContextAccessor httpContextAccessor)
        {
            _baseUserRepository = baseUserRepository;
            _mapper = mapper;
            _configuration = configuration;
            _userRepository = userRepository;
            _baseRefreshTokenRepository = baseRefreshTokenRepository;
            _confirmEmailRepository = confirmEmailRepository;
            _emailService = emailService;
            _basePermissionRepository = basePermissionRepository;
            _baseRoleRepository = baseRoleRepository;
            _baseTeamRepository = baseTeamRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public AuthService() { }
        public async Task<ResponseObject<DataResponseLogin>> GetJwtTokenAsync(User user)
        {
            var permissions = await _basePermissionRepository.GetAllAsync(x => x.UserId == user.Id);
            var roles = await _baseRoleRepository.GetAllAsync();

            var authClaims = new List<Claim>
    {
        new Claim("Id", user.Id.ToString()),
        new Claim("UserName", user.UserName),
        new Claim("Email", user.Email),
        new Claim("Avatar", user.Avatar),
        new Claim("FullName", user.FullName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            foreach (var permission in permissions)
            {
                foreach (var role in roles)
                {
                    if (role.Id == permission.RoleId)
                    {
                        authClaims.Add(new Claim("Permission", role.RoleName));
                    }
                }
            }

            var userRoles = await _userRepository.GetRolesOfUserAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);

            RefreshToken rf = new RefreshToken
            {
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddHours(refreshTokenValidity),
                Token = refreshToken
            };

            rf = await _baseRefreshTokenRepository.CreateAsync(rf);

            return new ResponseObject<DataResponseLogin>
            {
                Status = StatusCodes.Status200OK,
                Message = "Token created successfully",
                Data = new DataResponseLogin
                {
                    Id = rf.Id,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    RefreshToken = refreshToken,
                    User = new DataResponseUser
                    {
                        CreateTime = user.CreateTime,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        FullName = user.FullName,
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                    }
                }
            };
        }

        public async Task<ResponseObject<DataResponseLogin>> Login(Request_Login request)
        {
            try
            {
                var user = await _userRepository.GetUserByUsername(request.Username);
                if (user == null)
                {
                    return new ResponseObject<DataResponseLogin>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Tài khoản không chính xác",
                        Data = null
                    };
                }
                var checkPassword = BcryptNet.Verify(request.Password, user.Password);
                if (!checkPassword)
                {
                    return new ResponseObject<DataResponseLogin>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Mật khẩu không chính xác",
                        Data = null
                    };
                }
                return new ResponseObject<DataResponseLogin>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Đăng nhập thành công",
                    Data = new DataResponseLogin
                    {
                        AccessToken = GetJwtTokenAsync(user).Result.Data.AccessToken,
                        Id = user.Id,
                        RefreshToken = GetJwtTokenAsync(user).Result.Data.RefreshToken,
                        User = new DataResponseUser
                        {
                            CreateTime = user.CreateTime,
                            DateOfBirth = user.DateOfBirth,
                            Email = user.Email,
                            FullName = user.FullName,
                            Id = user.Id,
                            PhoneNumber = user.PhoneNumber,
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseLogin>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseObject<DataResponseUser>> Register(Request_Register request)
        {
            try
            {
                if (await _userRepository.GetUserByEmail(request.Email) != null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Email đã tồn tại trên hệ thống",
                        Data = null
                    };
                }
                if (await _userRepository.GetUserByPhoneNumber(request.PhoneNumber) != null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Số điện thoại đã tồn tại trên hệ thống",
                        Data = null
                    };
                }
                if (await _userRepository.GetUserByUsername(request.Username) != null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Tên tài khoản đã tồn tại",
                        Data = null
                    };
                }
                if (!ValidateInput.IsValidEmail(request.Email))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Định dạng email không hợp lệ",
                        Data = null
                    };
                }
                if (!ValidateInput.IsValidPhoneNumber(request.PhoneNumber))
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Định dạng số điện thoại không hợp lệ",
                        Data = null
                    };
                }
                var user = new User
                {
                    Avatar = "https://i.kym-cdn.com/entries/icons/facebook/000/017/666/avatar_default_big.jpg",
                    IsActive = true,
                    CreateTime = DateTime.Now,
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    FullName = request.FullName,
                    Id = Guid.NewGuid(),
                    Gender = request.Gender,
                    Password = BcryptNet.HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.Username,
                    TeamId = request.TeamId
                };
                user = await _baseUserRepository.CreateAsync(user);
                await _userRepository.AddUserToRoleAsync(user, new List<string> { "Employee" });

                var team = await _baseTeamRepository.GetAsync(x => x.Id == user.TeamId);
                team.NumberOfMember = await _baseTeamRepository.CountAsync(x => x.Id == user.TeamId);
                await _baseTeamRepository.UpdateAsync(team);

                
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Đăng ký tài khoản thành công",
                    Data = new DataResponseUser
                    {
                        Id = user.Id,
                        CreateTime = user.CreateTime,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        Avatar = user.Avatar,
                        Gender = user.Gender.ToString(),
                        TeamName = _baseTeamRepository.GetAsync(x => x.Id == user.TeamId).Result.Name,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = "Error: " + ex.Message,
                    Data = null
                };
            }
        }
        

        public async Task<string> ChangePassword(Guid userId, Request_ChangePassword request)
        {
            try
            {
                var user = await _baseUserRepository.GetByIDAsync(userId); // lấy user theo id
                bool checkPassword = BcryptNet.Verify(request.OldPassword, user.Password); // check mật khẩu cũ mà mình truyền vào có trùng với mật khẩu trong db hay không
                if (!checkPassword)
                {
                    return "Mật khẩu không chính xác"; // mật khẩu không chính xác
                }
                if (!request.NewPassword.Equals(request.ConfirmPassword))
                {
                    return "Mật khẩu không trùng khớp";
                }
                // nếu chính xác thì gán mật khẩu mới vào
                user.Password = BcryptNet.HashPassword(request.NewPassword);
                await _baseUserRepository.UpdateAsync(user);
                return "Đổi mật khẩu thành công";

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        private string GenerateCodeActive()
        {
            return "InkMastery" + "_" + DateTime.Now.Ticks.ToString();
        }
        public async Task<string> ForgotPassword(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return "Người dùng không tồn tại";
                }
                await _confirmEmailRepository.DeleteAsync(x => x.UserId == user.Id);
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    ConfirmCode = GenerateCodeActive(),
                    ExpiryTime = DateTime.Now.AddMinutes(1),
                    Id = Guid.NewGuid(),
                    IsConfirm = false,
                    UserId = user.Id
                };
                confirmEmail = await _confirmEmailRepository.CreateAsync(confirmEmail);
                var message = new EmailMessage(new string[] { email }, "Nhận mã xác nhận tại đây: ", $"Mã xác nhận: {confirmEmail.ConfirmCode}");
                var responseMessage = _emailService.SendEmail(message);
                return "Mã xác nhận đã được gửi về email của bạn! Vui lòng check Email";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<string> ConfirmCreateNewPassword(Request_ConfirmCreateNewPassword request)
        {
            try
            {
                var confirmEmail = await _confirmEmailRepository.GetAsync(x => x.ConfirmCode.Equals(request.ConfirmCode));
                if (confirmEmail == null)
                {
                    return "Mã xác nhận không hợp lệ";
                }
                if (confirmEmail.ExpiryTime < DateTime.Now)
                {
                    return "Mã xác nhận đã hết hạn";
                }
                var user = await _baseUserRepository.GetByIDAsync(confirmEmail.UserId);
                if (!request.Password.Equals(request.ConfirmPassword))
                {
                    return "Mật khẩu không trùng khớp";
                }
                user.Password = BcryptNet.HashPassword(request.Password);
                await _baseUserRepository.UpdateAsync(user);
                confirmEmail.IsConfirm = true;
                await _confirmEmailRepository.UpdateAsync(confirmEmail);
                return "Tạo mật khẩu mới thành công";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<ResponseObject<DataResponseUser>> AddRoleToUser(Guid userId, List<string> roles)
        {
            try
            {
                var user = await _baseUserRepository.GetByIDAsync(userId);
                if (user == null)
                {
                    return new ResponseObject<DataResponseUser>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Không tìm thấy thông tin người dùng",
                        Data = null
                    };
                }
                await _userRepository.AddUserToRoleAsync(user, roles);
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Thêm quyền hạn cho người dùng thành công",
                    Data = _mapper.EntityToDTOForUser(user)
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject<DataResponseUser>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IQueryable<DataResponseRole>> GetAllRoles()
        {
            var query = _baseRoleRepository.GetAllAsync(x => x.IsActive == true);
            return query.Result.Select(x => new DataResponseRole
            {
                Id = x.Id,
                RoleCode = x.RoleCode,
                RoleName = x.RoleName
            });
        }
        public async Task<IQueryable<string>> GetRolesByUserId(Guid userId)
        {
            var user = await _baseUserRepository.GetByIDAsync(userId);
            var query = await _userRepository.GetRolesOfUserAsync(user);
            return query.AsQueryable();
        }

        #region PrivateMethods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var expirationTimeUtc = DateTime.UtcNow.AddHours(tokenValidityInMinutes);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTimeUtc, localTimeZone);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationTimeInLocalTimeZone,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            return principal;

        }
        #endregion
    }
}
