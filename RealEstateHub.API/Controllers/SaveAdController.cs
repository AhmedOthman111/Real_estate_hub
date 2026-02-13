using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.SaveAd.Command;
using RealEstateHub.Application.CQRS.SaveAd.Queries;
using System.Security.Claims;

namespace RealEstateHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveAdController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SaveAdController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetAppUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        /// <summary>
        /// Toggles the saved/unsaved state of an ad for the authenticated customer.
        /// </summary>
        /// <param name="adId">The ID of the ad to save or unsave.</param>
        /// <returns>A confirmation message that the favorite has been toggled.</returns>
        /// <remarks>
        /// - This endpoint acts as a toggle switch for saving/unsaving an ad.
        /// - If the ad is not currently saved, it will be added to the customer's saved list.
        /// - If the ad is already saved, it will be removed from the customer's saved list.
        /// - The ad must exist and be in an active state to be saved.
        /// - There is no limit to the number of ads a customer can save.
        /// - The saved state is specific to the authenticated customer and does not affect other users.
        /// - Requires the "Customer" role.
        /// </remarks>
        /// <response code="200">Favorite toggled successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Customer role.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpPost("{adId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ToggleSaveAd(int adId)
        {
            var customerId = GetAppUserId();

            await _mediator.Send(new ToggleSaveAdCommand(adId, customerId));

            return Ok(new { Message = "Favorite toggled successfully." });
        }



        /// <summary>
        /// Retrieves all saved ads for the authenticated customer.
        /// </summary>
        /// <returns>A list of ads card saved by the authenticated customer.</returns>
        /// <remarks>

        /// - Returns all ads that the authenticated customer has saved/bookmarked.
        /// - The response includes full ad details such as:
        ///   - **Ad ID**: The unique identifier of the saved ad.
        ///   - **Title**: The title of the ad.
        ///   - **perpose**: The perpose of ad as sale or rent.
        ///   - **Category**: as محل, شقه,فيلا.
        ///   - **Price**: The listed price of the ad.
        /// - Returns an empty list if the customer has no saved ads.
        /// - Ads that have been deactivated, expired, or deleted by the owner may still appear in the saved list
        ///   but with an indication of their current status.
        /// - Use the toggle endpoint (POST /api/saved-ads/{adId}) to remove an ad from the saved list.
        /// - Requires the "Customer" role.
        /// </remarks>
        /// <response code="200">Returns a list of saved ads for the authenticated customer.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Customer role.</response>

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetSaveAd()
        {
            var customerId = GetAppUserId();

            var result = await _mediator.Send(new GetMySavedQuery(customerId));

            return Ok(result);
        }


    }
}
