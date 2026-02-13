using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.CommentReply.Commands;
using RealEstateHub.Application.CQRS.CommentReply.DTO;
using RealEstateHub.Application.CQRS.CommentReply.Queries;
using System.Security.Claims;

namespace RealEstateHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private string GetAppUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }


        /// <summary>
        /// Creates a new comment on an ad by Customer.
        /// </summary>
        /// <param name="dto">The comment creation details as adId and Comment Content.</param>
        /// <returns>The created comment details.</returns>
        /// <remarks>
        /// - Only customers can create comments on ads.
        /// - The authenticated customer will be assigned as the comment author automatically.
        /// - Comments are publicly visible on the ad listing.
        /// - The ad must exist and be in an active state to accept comments.
        /// - The comment content must not be empty or exceed the maximum character limit.
        /// - Requires the "Customer" role.
        /// </remarks>
        /// <response code="200">Comment created successfully. Returns the created comment details.</response>
        /// <response code="400">If the comment data is invalid or ad status is not active.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Customer role.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto)
        {
            var customerAppUserId = GetAppUserId();

            var reuslt = await _mediator.Send(new CreateCommentCommand(dto, customerAppUserId));

            return Ok(reuslt);
        }

        /// <summary>
        /// Creates a reply to an existing comment on an ad by owner.
        /// </summary>
        /// <param name="dto">The reply details as comment id and reply content.</param>
        /// <returns>The created reply details.</returns>
        /// <remarks>
        /// - Only the ad owner can reply to comments on their own ads.
        /// - The authenticated owner will be assigned as the reply author automatically.
        /// - Replies are publicly visible under the parent comment on the ad listing.
        /// - The parent comment must exist and belong to an ad owned by the authenticated owner.
        /// - The reply content must not be empty or exceed the maximum character limit.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="200">Reply created successfully. Returns the created reply details.</response>
        /// <response code="400">If the reply data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role or does not own the ad.</response>
        /// <response code="404">If the parent comment was not found.</response>

        [HttpPost("reply")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateReply([FromBody] ReplyCommentDto dto)
        {
            var AppUserOwnerId = GetAppUserId();

            var result = await _mediator.Send(new ReplyCommentCommand(dto, AppUserOwnerId));

            return Ok(result);
        }

        /// <summary>
        /// Deletes a comment made by the authenticated customer.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <remarks>
        /// - Only the customer who created the comment can delete it.
        /// - This action is irreversible; the comment will be permanently removed.
        /// - Any replies associated with this comment may also be removed.
        /// - The comment must exist and belong to the authenticated customer.
        /// - Requires the "Customer" role.
        /// </remarks>
        /// <response code="204">Comment deleted successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Customer role or does not own the comment.</response>
        /// <response code="404">If the comment was not found.</response>

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var AppUserCustomerId = GetAppUserId();
            await _mediator.Send(new DeleteCommentCommand(id, AppUserCustomerId));

            return NoContent();
        }


        /// <summary>
        /// Deletes a reply made by the authenticated owner.
        /// </summary>
        /// <param name="id">The ID of the reply to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <remarks>
        /// - Only the owner who created the reply can delete it.
        /// - This action is irreversible; the reply will be permanently removed.
        /// - The reply must exist and belong to the authenticated owner.
        /// - Deleting a reply does not affect the parent comment.
        /// - Requires the "Owner" role.
        /// </remarks>
        /// <response code="204">Reply deleted successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role or does not own the reply.</response>
        /// <response code="404">If the reply was not found.</response>

        [HttpDelete("reply/{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteReply(int id)
        {
            var AppUserOwnerId = GetAppUserId();
            await _mediator.Send(new DeleteReplyCommand(id, AppUserOwnerId));

            return NoContent();
        }



        /// <summary>
        /// Retrieves all comments and their replies for a specific ad.
        /// </summary>
        /// <param name="adId">The ID of the ad to retrieve comments for.</param>
        /// <returns>A list of comments and their associated replies for the specified ad.</returns>
        /// <remarks>
        /// - This endpoint is publicly accessible and does not require authentication.
        /// - Returns all comments associated with the specified ad.
        /// - Each comment includes its nested replies from the ad owner.
        /// - Comments are typically ordered by creation date (newest first or oldest first).
        /// - The response includes comment details such as:
        ///   - **Comment ID**: The unique identifier of the comment.
        ///   - **Content**: The text content of the comment.
        ///   - **Author Name**: The name of the customer who posted the comment.
        ///   - **Created Date**: The date and time the comment was posted.
        ///   - **Replies**: A list of owner replies associated with each comment.
        /// - Returns an empty list if no comments exist for the specified ad.
        /// </remarks>
        /// <response code="200">Returns a list of comments and replies for the ad.</response>
        /// <response code="404">If the ad was not found.</response>

        [HttpGet("{adId}")]
        public async Task<IActionResult> GetByAdId(int adId)
        {
            var result = await _mediator.Send(new GetAdCommentsQuery(adId));
            return Ok(result);
        }



    }
}
