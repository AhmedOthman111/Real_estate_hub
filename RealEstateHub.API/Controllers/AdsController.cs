using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.Ads.Commands;
using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Application.CQRS.Ads.Queries;
using System.Security.Claims;

namespace RealEstateHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetAppUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }



        /// <summary>
        /// Used by Owner to Creates a new ad.
        /// </summary>
        /// <param name="dto">The ad creation details.</param>
        /// <returns>The created ad details.</returns>
        /// <remarks>
        /// - The ad will be created with a "Pending" status and must be approved by an admin before it becomes visible.
        /// - All required fields must be provided; otherwise, a validation error will be returned.
        /// - Photos can be uploaded separately using the photo upload endpoint after the ad is created.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="200">Ad created successfully. Returns the created ad details.</response>
        /// <response code="400">If the ad data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateAd([FromBody] CreateAdDto dto)
        {
            var AUId = GetAppUserId();

            var result = await _mediator.Send(new CreateAdCommand(dto, AUId));

            return Ok(result);
        }



        /// <summary>
        /// used by Owner to Updates an existing ad .
        /// </summary>
        /// <param name="id">The ID of the ad to update.</param>
        /// <param name="dto">The updated ad details.</param>
        /// <returns>The updated ad details.</returns>
        /// <remarks>
        /// - The ID in the URL must match the ID in the request body; otherwise, a 400 error will be returned.
        /// - Only the owner of the ad can update it.
        /// - All required fields must be provided in the update payload.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="200">Ad updated successfully. Returns the updated ad details.</response>
        /// <response code="400">If the ID mismatch or the ad data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateAd([FromRoute] int id, [FromBody] UpdateAdDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var AUId = GetAppUserId();
            var result = await _mediator.Send(new UpdateAdCommand(dto, AUId));
            return Ok(result);
        }


        /// <summary>
        /// used by owner to Deletes an existing ad .
        /// </summary>
        /// <param name="id">The ID of the ad to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <remarks>
        /// - Only the owner of the ad can delete it.
        /// - This action is irreversible; the ad and all associated data (including photos) will be permanently removed.
        /// - Any  Saved associated with this ad will also be removed.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="204">Ad deleted successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteAd([FromRoute] int id)
        {
            var AUId = GetAppUserId();
            await _mediator.Send(new DeleteAdCommand(id, AUId));
            return NoContent();
        }

        /// <summary>
        /// change  the status of Ad to Sold or rented .
        /// </summary>
        /// <param name="id">The ID of the ad to change status.</param>
        /// <returns>No content if the status change was successful.</returns>
        /// <remarks>
        /// - Only the owner of the ad can change its status.
        /// - The Status of ad should be as Active to change it.
        /// - This endpoint changes the ad status to "Sold" or "Rented".
        /// - An "Sold" and "Rented" ad will not appear in public search results.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="204">Ad status changed successfully.</response>
        /// <response code="400">If the status of ad not Active.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>
        /// <response code="404">If the ad was not found.</response>


        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id)
        {
            var AUId = GetAppUserId();
            await _mediator.Send(new ChangeAdStatusCommand(id, AUId));
            return NoContent();
        }



        /// <summary>
        /// Renews an ad for a specified number of days.
        /// </summary>
        /// <param name="id">The ID of the ad to renew.</param>
        /// <param name="durationdays">The number of days to extend the ad's active period.</param>
        /// <returns>No content if the renewal was successful.</returns>
        /// <remarks>
        /// - Only the owner of the ad can renew it.
        /// - The duration is added to the current expiration date of the ad.
        /// - If the ad has already expired, the duration will be calculated from the current date.
        /// - The duration must be a positive integer representing the number of days.
        /// - it changes Status to active if it was expired.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="204">Ad renewed successfully.</response>
        /// <response code="400">If the duration is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>
        /// <response code="404">If the ad was not found.</response>

        [Authorize(Roles = "Owner")]
        [HttpPatch("{id:int}/renew")]
        public async Task<IActionResult> RenewAd([FromRoute] int id, [FromQuery] int durationdays)
        {
            var AUOwnerId = GetAppUserId();

            await _mediator.Send(new RenewAdCommand(id, durationdays, AUOwnerId));

            return NoContent();
        }


        /// <summary>
        /// Retrieves all ads belonging to the authenticated owner.
        /// </summary>
        /// <returns>A list of the owner's ads.</returns>
        /// <remarks>
        /// - Returns all ads owned by the authenticated user regardless of their status (Active, Inactive, Pending, Expired, Rejected).
        /// - The results include ad details such as title, price, status, creation date, and expiration date.
        /// - Photos associated with each ad are also included in the response.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="200">Returns a list of the owner's ads.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>


        [Authorize(Roles = "Owner")]
        [HttpGet("my-ads")]
        public async Task<IActionResult> GetMyAds()
        {
            var AUOwnerId = GetAppUserId();

            var result = await _mediator.Send(new GetMyAdsQuery(AUOwnerId));

            return Ok(result);
        }



        /// <summary>
        /// Retrieves a specific ad by its ID.
        /// </summary>
        /// <param name="id">The ID of the ad to retrieve.</param>
        /// <returns>The ad details.</returns>
        /// <remarks>
        /// - This endpoint is publicly accessible and does not require authentication.
        /// - Returns the full ad details including title, description, price, photos, comments, replys and owner information.
        /// </remarks>
        /// <response code="200">Returns the ad details.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAdById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetAdByIdQuery(id));
            return Ok(result);

        }


        /// <summary>
        /// Retrieves a filtered and paginated list of ads .
        /// </summary>
        /// <param name="filter">The filter and pagination parameters.</param>
        /// <returns>A filtered list of ads.</returns>
        /// <remarks>
        /// - This endpoint is publicly accessible and does not require authentication.
        /// - All filter parameters are optional; if none are provided, all active approved ads are returned.
        /// - Supports pagination with PageNumber and PageSize parameters.
        /// - Supports sorting by various fields such as price, date, and title.
        /// - Only approved and active ads are returned in the results.
        /// - Available filter parameters:
        ///   - **City**: Filter by city name.
        ///   - **perpose**: Filter by sale or rent.
        ///   - **MinArea / MaxArea**: Filter by unit area range.
        ///   - **MinPrice / MaxPrice**: Filter by price range.
        ///   - **CategoryId**: Filter by category.
        ///   - **SearchTerm**: Search in title and description.
        ///   - **PageNumber / PageSize**: Pagination controls.
        /// </remarks>
        /// <response code="200">Returns a filtered list of ads.</response>
        /// <response code="400">If the filter parameters are invalid.</response>

        [HttpGet]
        public async Task<IActionResult> GetAds([FromQuery] AdFilterDto filter)
        {
            var result = await _mediator.Send(new GetAdsQuery(filter));
            return Ok(result);
        }


        /// <summary>
        /// Retrieves all ads with a pending approval status.
        /// </summary>
        /// <returns>A list of pending ads awaiting approval.</returns>
        /// <remarks>
        /// - This endpoint is restricted to administrators only.
        /// - Returns all ads that are in "Pending" status and awaiting admin review.
        /// - Admins can then approve or reject each ad using the respective endpoints.
        /// - The response includes all ad details along with owner information.
        /// - Requires the "Admin" role.
        /// </remarks>
        /// <response code="200">Returns a list of pending ads.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Admin role.</response>

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingAds()
        {
            var pendingads = await _mediator.Send(new GetPendingAdsQuery());
            return Ok(pendingads);
        }


        /// <summary>
        /// Approves a pending ad, making it visible to the public.
        /// </summary>
        /// <param name="id">The ID of the ad to approve.</param>
        /// <returns>No content if the approval was successful.</returns>
        /// <remarks>
        /// - This endpoint is restricted to administrators only.
        /// - Only ads with a "Pending" status can be approved.
        /// - Once approved, the ad becomes active and visible in public search results.
        /// - The ad owner receive a mail notification that their ad has been approved.
        /// - The ad's expiration timer starts from the moment of approval.
        /// - Requires the "Admin" role.
        /// </remarks>
        /// <response code="204">Ad approved successfully.</response>
        /// <response code="400">If the status of ad is not Pending.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Admin role.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveAd(int id)
        {
            await _mediator.Send(new ApproveAdCommand(id));
            return NoContent();
        }


        /// <summary>
        /// Rejects a pending ad  .
        /// </summary>
        /// <param name="id">The ID of the ad to reject.</param>
        /// <param name="dto">The optional rejection details The rejection reason.</param>
        /// <returns>No content if the rejection was successful.</returns>
        /// <remarks>
        /// - This endpoint is restricted to administrators only.
        /// - Only ads with a "Pending" status can be rejected.
        /// - A rejection reason is optional provided to inform the ad owner why their ad was not approved.
        /// - The ad owner receive a mail notification with the rejection reason.
        /// - Requires the "Admin" role.
        /// </remarks>
        /// <response code="204">Ad rejected successfully.</response>
        /// <response code="400">If the rejection reason is missing or invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Admin role.</response>
        /// <response code="404">If the ad was not found.</response>

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectAd(int id, [FromBody] RejectAdDto dto)
        {
            await _mediator.Send(new RejectAdCommand(id, dto.RejectionReason));
            return NoContent();
        }


    }
}
