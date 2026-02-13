using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.Profile.Command;
using RealEstateHub.Application.CQRS.Profile.Dto;
using RealEstateHub.Application.CQRS.Profile.Queries;
using System.Security.Claims;

namespace RealEstateHub.API.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetAppUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }



        /// <summary>
        /// Retrieves the authenticated customer's profile information.
        /// </summary>
        /// <remarks>
        /// Requires Customer role authorization.
        /// The customer ID is extracted from the authenticated user's claims.
        /// </remarks>
        /// <response code="200">Returns the customer profile data.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Customer role.</response>

        [HttpGet("customer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyCustomerProfile()
        {
            var customerid = GetAppUserId();

            var result = await _mediator.Send(new GetMyCustomerProfileQuery(customerid));

            return Ok(result);
        }



        /// <summary>
        /// Updates the authenticated customer's profile information.
        /// </summary>
        /// <remarks>
        /// Requires Customer role authorization.
        /// The customer ID is retrieved from the authenticated user's claims.
        /// </remarks>
        /// <param name="dto">Updated customer profile data.</param>
        /// <response code="204">Profile updated successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Customer role.</response>
        /// 

        [HttpPut("customer")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> UpdateCustomerProfile([FromBody] UpdateCustomerProfileDto dto)
        {
            var customerid = GetAppUserId();
            await _mediator.Send(new UpdateCustomerProfileCommand(customerid, dto));
            return NoContent();
        }



        /// <summary>
        /// Retrieves the public profile of an owner.
        /// </summary>
        /// <remarks>
        /// This endpoint does not require authentication.
        /// Returns publicly visible profile information for the specified owner.
        /// </remarks>
        /// <param name="id">The unique identifier of the owner.</param>
        /// <response code="200">Returns the owner's public profile data.</response>
        /// <response code="404">Owner not found.</response>

        [HttpGet("owner/{id}")]
        public async Task<IActionResult> GetOwnerProfile(int id)
        {
            var result = await _mediator.Send(new GetOwnerProfileQuery(id));
            return Ok(result);
        }



        /// <summary>
        /// Retrieves the authenticated owner's profile with sensetive data.
        /// </summary>
        /// <remarks>
        /// Requires Owner role authorization.
        /// The owner ID is extracted from the authenticated user's claims.
        /// </remarks>
        /// <response code="200">Returns the owner's profile data.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Owner role.</response>

        [HttpGet("owner/me")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetMyOwnerProfile()
        {
            var ownerId = GetAppUserId();
            var result = await _mediator.Send(new GetMyOwnerProfileQuery(ownerId));
            return Ok(result);
        }



        /// <summary>
        /// Updates the authenticated owner's profile information.
        /// </summary>
        /// <remarks>
        /// Requires Owner role authorization.
        /// The owner ID is retrieved from the authenticated user's claims.
        /// </remarks>
        /// <param name="dto">Updated owner profile data.</param>
        /// <response code="204">Profile updated successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="401">Unauthorized if the user is not authenticated.</response>
        /// <response code="403">Forbidden if the user does not have the Owner role.</response>

        [HttpPut("owner")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateOwnerProfile([FromBody] UpdateOwnerProfileDto dto)
        {
            var ownerId = GetAppUserId();
            await _mediator.Send(new UpdateOwnerProfileCommand(ownerId, dto));
            return NoContent();
        }


    }
}
 