using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.Rating.Command;
using RealEstateHub.Application.CQRS.Rating.Dto;
using RealEstateHub.Application.CQRS.Rating.Queries;
using System.Security.Claims;

namespace RealEstateHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RatingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetAppUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }




        /// <summary>
        /// Creates a new rating for an ad owner.
        /// </summary>
        /// <param name="dto">The rating creation details.</param>
        /// <returns>The created rating details.</returns>
        /// <remarks>
        /// - Only customers can create ratings for ad owners.
        /// - The authenticated customer will be assigned as the rating author automatically.
        /// - The rating value must be within the accepted range (e.g., 1 to 5 stars).
        /// - A customer can only rate a specific owner once; duplicate ratings are not allowed.
        /// - The review text is optional but recommended to provide feedback context.
        /// - The owner's overall average rating will be recalculated after each new rating.
        /// - Ratings are publicly visible on the owner's profile.
        /// - Requires the "Customer" role.
        /// </remarks>
        /// <response code="200">Rating created successfully. Returns the created rating details.</response>
        /// <response code="400">If the rating data is invalid or the customer has already rated this owner.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Customer role.</response>
        /// <response code="404">If the owner was not found.</response>

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateRate([FromBody] CreateRatingDto dto)
        {
            var customerid = GetAppUserId();

            var result = await _mediator.Send(new CreateRatingCommand(dto, customerid));

            return Ok(result);
        }


        /// <summary>
        /// Retrieves all ratings for a specific ad owner.
        /// </summary>
        /// <param name="ownerId">The ID of the owner to retrieve ratings for.</param>
        /// <returns>A list of ratings and reviews for the specified owner.</returns>
        /// <remarks>
        /// - This endpoint is publicly accessible and does not require authentication.
        /// - Returns all ratings and reviews associated with the specified owner.
        /// - The response includes rating details such as:
        ///   - **Rating ID**: The unique identifier of the rating.
        ///   - **Value**: The star rating value (e.g., 1 to 5).
        ///   - **Review**: The optional text review left by the customer.
        ///   - **Customer Name**: The name of the customer who submitted the rating.
        ///   - **Created Date**: The date and time the rating was submitted.
        /// - Ratings are typically ordered by creation date (newest first).
        /// - Returns an empty list if no ratings exist for the specified owner.
        /// </remarks>
        /// <response code="200">Returns a list of ratings for the specified owner.</response>
        /// <response code="404">If the owner was not found.</response>

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwnerId(int ownerId)
        {
            var result = await _mediator.Send(new GetOwnerRatingsQuery(ownerId));
            return Ok(result);
        }


    }
}
