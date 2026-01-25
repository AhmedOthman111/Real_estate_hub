using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.CQRS.AdPhotos.Commands;
using System.Security.Claims;

namespace RealEstateHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdPhotosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdPhotosController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private string GetAppUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("{adId}")]
        public async Task<IActionResult> UploadPhoto(int adId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var ownerid = GetAppUserId();

            using var stream = file.OpenReadStream();

            var photoUrl = await _mediator.Send(new UploadAdPhotoCommand(adId, stream, file.FileName, ownerid));

            return Ok(new { Url = photoUrl });
        }


        [Authorize(Roles = "Owner")]
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var ownerid = GetAppUserId();

            await _mediator.Send(new DeleteAdPhotoCommand(photoId, ownerid));

            return NoContent();
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{photoId}/main")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            var ownerid = GetAppUserId();
            await _mediator.Send(new SetMainAdPhotoCommand(photoId, ownerid));
            return NoContent();
        }
    }
}
