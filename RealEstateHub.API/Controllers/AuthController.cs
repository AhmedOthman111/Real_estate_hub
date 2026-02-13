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


        /// <summary>
        /// Registers a new customer account.
        /// </summary>
        /// <param name="dto">The customer registration required details.</param>
        /// <returns>The registration result.</returns>
        /// <response code="200">Customer registered successfully.</response>
        /// <response code="400">If the registration data is invalid or if email or username is allready created.</response>

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer ( [FromBody] RegisterCustomerDto dto)
        {
            var result = await _mediator.Send(new RegisterCustomerCommand(dto));
            return Ok(result);
        }


        /// <summary>
        /// Registers a new owner account.
        /// </summary>
        /// <param name="dto">The owner registration required details.</param>
        /// <returns>The registration result.</returns>
        /// <response code="200">Owner registered successfully.</response>
        /// <response code="400">If the registration data is invalid or if email or username is allready created.</response>

        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterOwner([FromBody] RegisterOwnerDto dto)
        {
            var result = await _mediator.Send(new RegisterOwnerCommand(dto));
            return Ok(result);
        }


        /// <summary>
        /// Confirms a user's email address using the provided token.
        /// </summary>
        /// <remarks>
        /// This endpoint is called auto from mail that user recived when account is created.
        /// </remarks>
        /// <param name="email">The email address to confirm.</param>
        /// <param name="token">The email confirmation token.</param>
        /// <returns>The email confirmation result.</returns>
        /// <response code="200">Email confirmed successfully.</response>
        /// <response code="400">If the email or token is invalid.</response>

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email  , [FromQuery] string token)
        {
            var dto = new ConfirmEmailDto() {UserEmail = email ,Token = token };
            
            var result = await _mediator.Send(new ConfirmEmailCommand(dto));

            return Ok(result);
        }


        /// <summary>
        /// Authenticates a user and returns a login token.
        /// </summary>
        /// <remarks>
        /// - Usser can login using Email or UserName.
        /// - The user's email must be confirmed before logging in.
        /// - Returns a JWT token upon successful authentication.
        /// - The token should be included in the Authorization header for subsequent requests:
        ///   `Authorization: Bearer {token}`
        /// </remarks>
        /// <param name="dto">The login credentials.</param>
        /// <returns>The authentication result including the token.</returns>
        /// <response code="200">Login successful. Returns authentication token.</response>
        /// <response code="401">If the credentials are invalid.</response>

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));

            if (!result.Success) return Unauthorized(result.Message);

            return Ok(result);
        }


        /// <summary>
        /// Sends a password reset OTP to the specified email address.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        /// <returns>A confirmation message that the OTP has been sent if the account exists.</returns>
        /// <remarks>
        /// - An OTP (One-Time Password) will be sent to the provided email address if an account exists.
        /// - The OTP has a limited validity period and can only be used once.
        /// - Use the OTP received in the email along with the reset-password endpoint to complete the process.
        /// </remarks>
        /// <response code="200">Password reset OTP sent successfully (if account exists).</response>
        /// <response code="400">If the email is invalid or empty.</response>

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            await _mediator.Send(new ForgotPasswordCommand(email));
            return Ok("If an account with that email exists, a password reset OTP has been sent.");
        }



        /// <summary>
        /// Resets the user's password using the provided OTP and new password.
        /// </summary>
        /// <param name="dto">The password reset details including email ,OTP ,new password and conferim new password  .</param>
        /// <returns>A confirmation message that the password has been reset.</returns>
        /// <remarks>
        /// - The OTP must be obtained from the forgot-password endpoint.
        /// - The OTP is valid for a limited time only.
        /// - The new password must meet the minimum complexity requirements.
        /// - After successful reset, the user can log in with the new password.
        /// </remarks>
        /// <response code="200">Password reset successfully.</response>
        /// <response code="400">If the reset data is invalid or the OTP is expired.</response>

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _mediator.Send(new ResetPasswordCommand(dto));
            return Ok("Password has been reset successfully");
        }


        /// <summary>
        /// Authenticates a user using their Google account OAuth.
        /// </summary>
        /// <param name="dto">The Google authentication details including the ID token and the role of him .</param>
        /// <returns>The authentication result including the token.</returns>
        /// <remarks>
        /// - The ID token must be obtained from Google's OAuth 2.0 authentication flow on the client side.
        /// - If the user does not have an existing account, one will be created automatically.
        /// - Email confirmation is not required for Google-authenticated accounts.
        /// - Returns a JWT token upon successful authentication.
        /// - The token should be included in the Authorization header for subsequent requests:
        ///   `Authorization: Bearer {token}`
        /// </remarks>
        /// <response code="200">Google login successful. Returns authentication token.</response>
        /// <response code="403">If the Google authentication fails or the account is forbidden.</response>

        [HttpPost("google-auth")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            var result = await _mediator.Send(new GoogleLoginCommand(dto));
            if (!result.Success) return  Forbid(result.Message);
            return Ok(result);
        }



    }
}