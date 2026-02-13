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



        /// <summary>
        /// Uploads a photo for a specific ad.
        /// </summary>
        /// <param name="adId">The ID of the ad to upload the photo for.</param>
        /// <param name="file">The image file to upload.</param>
        /// <returns>The URL of the uploaded photo.</returns>
        /// <response code="200">Returns the URL of the uploaded photo.</response>
        /// <response code="400">If no file was uploaded or the file is empty.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>

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


        /// <summary>
        /// Deletes a specific photo.
        /// </summary>
        /// <param name="photoId">The ID of the photo to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <response code="204">The photo was successfully deleted.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>
        /// <response code="404">If the photo was not found.</response>

        [Authorize(Roles = "Owner")]
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var ownerid = GetAppUserId();

            await _mediator.Send(new DeleteAdPhotoCommand(photoId, ownerid));

            return NoContent();
        }

        /// <summary>
        /// Sets a specific photo as the main photo for its associated ad.
        /// </summary>
        /// <param name="photoId">The ID of the photo to set as main.</param>
        /// <returns>No content if the operation was successful.</returns>
        /// <response code="204">The photo was successfully set as the main photo.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have the Owner role.</response>
        /// <response code="404">If the photo was not found.</response>

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
