using Google.Apis.Auth;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Pipelines.Sockets.Unofficial.Buffers;
using RealEstateHub.Application.Common;
using RealEstateHub.Application.CQRS.Auth.Dto;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace RealEstateHub.Infrastructure.ExternalServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;


        public AuthService(UserManager<AppUser> userManager , IUnitOfWork unitOfWork , IEmailService emailService, 
                            IConfiguration configuration , ITokenService tokenService)
        {
            _userManager = userManager;
            _uow = unitOfWork;
            _emailService = emailService;
            _config = configuration;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterCustomerAsync (RegisterCustomerDto dto)
        {
            var exitinguser = await _userManager.FindByEmailAsync (dto.Email);

            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                throw new BusinessException("Email already in use.");

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                throw new BusinessException("Username already in use.");

            var newuser = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(newuser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(newuser, "Customer");
           

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newuser);
           
            var confirmLink = $"{_config["App:BaseUrl"]}/api/auth/confirm-email?email={Uri.EscapeDataString(newuser.Email)}&token={Uri.EscapeDataString(token)}";

            BackgroundJob.Enqueue(() => _emailService.SendConfirmationEmailAsync(newuser.Email, confirmLink));

            return new AuthResponseDto {Success = true , Message = "Registration successful. Please confirm your email." , Email = newuser.Email};
        }

        public async Task<AuthResponseDto> RegisterOwnerAsync(RegisterOwnerDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                throw new BusinessException("Email already in use.");
            
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                throw new BusinessException("Username already in use.");

            var NewAppUser = new AppUser
            { 
               UserName = dto.UserName, 
               Email = dto.Email,
               NationalId = dto.NationalId,
               PhoneNumber = dto.PhoneNumber,
               FirstName = dto.FirstName,   
               LastName = dto.LastName,
            };

            var result = await _userManager.CreateAsync(NewAppUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(NewAppUser, "Owner");

            var owner = new Owner
            {
                AppUserId = NewAppUser.Id,
                Address = dto.Address,
                WhatsappNumber = dto.WhatsappNumber,
                Bio = dto.Bio ,
                CompanyName = dto.CompanyName ,
                AverageRating = 0,
            };
            await _uow.Owner.AddAsync(owner);
            await _uow.SaveChangesAsync();


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(NewAppUser);

            var confirmLink = $"{_config["App:BaseUrl"]}/api/auth/confirm-email?email={Uri.EscapeDataString(NewAppUser.Email)}&token={Uri.EscapeDataString(token)}";

            BackgroundJob.Enqueue(() => _emailService.SendConfirmationEmailAsync(NewAppUser.Email, confirmLink));

            return new AuthResponseDto {Success = true , Message = "Registration successful. Please confirm your email.", Email = NewAppUser.Email };

        }

        public async Task<AuthResponseDto> ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            var appuser = await _userManager.FindByEmailAsync(dto.UserEmail);
            if (appuser == null) throw new NotFoundException("User",dto.UserEmail);

            if (appuser.EmailConfirmed) throw new BusinessException("Email already confirmed"); 

            var result = await _userManager.ConfirmEmailAsync(appuser, dto.Token);
            if (!result.Succeeded)
                throw new  BusinessException("Invalid or expired token..");

            return new AuthResponseDto {Success = true ,  Email = dto.UserEmail, Message = "User Mail confirmed Successfully." };
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var appuser = await _userManager.FindByEmailAsync(dto.identity);

            if (appuser == null) 
                appuser =  await _userManager.FindByNameAsync(dto.identity);

            if (appuser == null) 
                return new AuthResponseDto { Success = false , Message = "Invalid credentials."};

            var valid = await _userManager.CheckPasswordAsync( appuser, dto.Password );
            if (!valid) return new AuthResponseDto { Success = false, Message = "Invalid credentials."};

            if (!appuser.EmailConfirmed)
            {
                var confermemailtoken = await _userManager.GenerateEmailConfirmationTokenAsync(appuser);
                var confirmLink = $"{_config["App:BaseUrl"]}/api/auth/confirm-email?email={Uri.EscapeDataString(appuser.Email)}&token={Uri.EscapeDataString(confermemailtoken)}";


                BackgroundJob.Enqueue(() => _emailService.SendConfirmationEmailAsync(appuser.Email, confirmLink));
                        
                return new AuthResponseDto { Success = true, Message = "Check your Bxo Mail to Conferm  your Account." };
            }

            var userroles = await _userManager.GetRolesAsync(appuser);

            var tokendto = new TokenUserDTO
            {
                Email = appuser.Email,
                FullName = appuser.FirstName + " " + appuser.LastName,
                Id = appuser.Id,
                Roles = userroles.ToList(),
                UserName = appuser.UserName
            };
            
            var token = await _tokenService.CreateTokenAsync(tokendto);

            return new AuthResponseDto
            {
                Success = true,
                Email = appuser.Email,
                Fullname = appuser.FirstName + " " + appuser.LastName,
                Message = "Login successful.",
                Token = token
            };

        }

        public async Task ForgotPasswordAsync (string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
                throw new NotFoundException("User", email);

            var otp = new Random().Next(100000, 999999).ToString();

            user.PasswordResetOtp = otp;
            user.PasswordResetOtpExpiry = DateTime.UtcNow.AddMinutes(5);
            user.PasswordResetOtpUsed = false;

            await _userManager.UpdateAsync(user);

            BackgroundJob.Enqueue(() => _emailService.SendOtpEmailAsync(email, otp));

        }
        public async Task ResetPasswordAsync (ResetPasswordDto dto)
        {
            var user  = await  _userManager.FindByEmailAsync(dto.Email);   
            if (user == null) throw new BusinessException("Invalid request");

            if(user.PasswordResetOtp != dto.Otp || user.PasswordResetOtpExpiry < DateTime.UtcNow || user.PasswordResetOtpUsed )
                throw new BusinessException("Invalid or expired OTP");

            var token =  await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await  _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Password reset failed: {errors}");
            }
          
            user.PasswordResetOtpUsed = true;
            await _userManager.UpdateAsync(user);
        }

        
        //validate google token and fitch userinfo from google token
        private async Task<ExternalUserInfoDto> ValidateGoogleTokenAsync(string idToken)

        {

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _config["Authentication:Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            if (payload == null) throw new Exception ("Faild to valid google tokn");

            return new ExternalUserInfoDto
            {
                Subject = payload.Subject,
                Email = payload.Email,
                FullName = payload.Name,
                IsEmailConfirmed = payload.EmailVerified
            };


        }

        public async Task<AuthResponseDto> ExternalLoginGoogleAsync(GoogleLoginDto dto)
        {
            var payload = await ValidateGoogleTokenAsync(dto.IdToken);

            if (!payload.IsEmailConfirmed)
                return new AuthResponseDto { Success = false, Message = "Your Google account email is not verified." };

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FirstName = payload.FullName,
                    EmailConfirmed = true,
                    Provider = "Google",
                    ProviderKey = payload.Subject
                };

                // Create user
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new BusinessException($"Registration failed: {errors}");
                }

                await _userManager.AddToRoleAsync(user, dto.role);

                if (dto.role == "Owner")
                {
                    var owner = new Owner { AppUserId = user.Id };
                    await _uow.Owner.AddAsync(owner);
                    await _uow.SaveChangesAsync();
                }
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokendto = new TokenUserDTO
            {
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Id = user.Id,
                Roles = roles.ToList(),
                UserName = user.UserName
            };

            var token = await _tokenService.CreateTokenAsync(tokendto);

            if (string.IsNullOrEmpty(user.PhoneNumber))
                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Message = "External login successful. Please complete your profile information."
                };

            return new AuthResponseDto { Success = true, Token = token, Message = "External login successful." };
        }

   


        

    }
}
