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


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto)
        {
            var customerAppUserId = GetAppUserId();

            var reuslt = await _mediator.Send(new CreateCommentCommand(dto, customerAppUserId));

            return Ok(reuslt);
        }


        [HttpPost("reply")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateReply([FromBody] ReplyCommentDto dto)
        {
            var AppUserOwnerId = GetAppUserId();

            var result = await _mediator.Send(new ReplyCommentCommand(dto, AppUserOwnerId));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var AppUserCustomerId = GetAppUserId();
            await _mediator.Send(new DeleteCommentCommand(id, AppUserCustomerId));

            return NoContent();
        }

        [HttpDelete("reply/{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteReply(int id)
        {
            var AppUserOwnerId = GetAppUserId();
            await _mediator.Send(new DeleteReplyCommand(id, AppUserOwnerId));

            return NoContent();
        }

        [HttpGet("{adId}")]
        public async Task<IActionResult> GetByAdId(int adId)
        {
            var result = await _mediator.Send(new GetAdCommentsQuery(adId));
            return Ok(result);
        }



    }
}
