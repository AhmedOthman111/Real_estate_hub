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

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateAd([FromBody] CreateAdDto dto)
        {
            var AUId = GetAppUserId();

            var result = await _mediator.Send(new CreateAdCommand(dto, AUId));

            return Ok(result);
        }


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateAd([FromRoute] int id, [FromBody] UpdateAdDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var AUId = GetAppUserId();
            var result = await _mediator.Send(new UpdateAdCommand(dto, AUId));
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteAd([FromRoute] int id)
        {
            var AUId = GetAppUserId();
            await _mediator.Send(new DeleteAdCommand(id, AUId));
            return NoContent();
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id)
        {
            var AUId = GetAppUserId();
            await _mediator.Send(new ChangeAdStatusCommand(id, AUId));
            return NoContent();
        }


        [Authorize(Roles = "Owner")]
        [HttpPatch("{id:int}/renew")]
        public async Task<IActionResult> RenewAd([FromRoute] int id, [FromQuery] int durationdays)
        {
            var AUOwnerId = GetAppUserId();

            await _mediator.Send(new RenewAdCommand(id, durationdays, AUOwnerId));

            return NoContent();
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("my-ads")]
        public async Task<IActionResult> GetMyAds()
        {
            var AUOwnerId = GetAppUserId();

            var result = await _mediator.Send(new GetMyAdsQuery(AUOwnerId));

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAdById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetAdByIdQuery(id));
            return Ok(result);

        }


        [HttpGet]
        public async Task<IActionResult> GetAds([FromQuery] AdFilterDto filter)
        {
            var result = await _mediator.Send(new GetAdsQuery(filter));
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingAds()
        {
            var pendingads = await _mediator.Send(new GetPendingAdsQuery());
            return Ok(pendingads);
        }


        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveAd(int id)
        {
            await _mediator.Send(new ApproveAdCommand(id));
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectAd(int id, [FromBody] RejectAdDto dto)
        {
            await _mediator.Send(new RejectAdCommand(id, dto.RejectionReason));
            return NoContent();
        }


    }
}
