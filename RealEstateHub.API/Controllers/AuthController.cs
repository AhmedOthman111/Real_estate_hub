using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.Auth.Commands;
using RealEstateHub.Application.CQRS.Auth.Dto;
using RealEstateHub.Application.CQRS.Auth.Handlers;

namespace RealEstateHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer ( [FromBody] RegisterCustomerDto dto)
        {
            var result = await _mediator.Send(new RegisterCustomerCommand(dto));
            return Ok(result);
        }
        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwnerDto dto)
        {
            var result = await _mediator.Send(new RegisterOwnerCommand(dto));
            return Ok(result);
        }
       
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email  , [FromQuery] string token)
        {
            var dto = new ConfirmEmailDto() {UserEmail = email ,Token = token };
            
            var result = await _mediator.Send(new ConfirmEmailCommand(dto));

            return Ok(result);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));

            if (!result.Success) return Unauthorized(result.Message);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            await _mediator.Send(new ForgotPasswordCommand(email));
            return Ok("If an account with that email exists, a password reset OTP has been sent.");
        }
        
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _mediator.Send(new ResetPasswordCommand(dto));
            return Ok("Password has been reset successfully");
        }


        [HttpPost("google-auth")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            var result = await _mediator.Send(new GoogleLoginCommand(dto));
            if (!result.Success) return  Forbid(result.Message);
            return Ok(result);
        }



    }
}