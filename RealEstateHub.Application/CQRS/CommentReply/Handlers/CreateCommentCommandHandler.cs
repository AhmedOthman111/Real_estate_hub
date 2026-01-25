using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.Commands;
using RealEstateHub.Application.CQRS.CommentReply.DTO;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.CommentReply.Handlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponseDto>
    {
        private readonly ICommentReplyService _commentService;
        private readonly ICacheService _cacheService;
        public CreateCommentCommandHandler(ICommentReplyService commentService, ICacheService cacheService)
        {
            _commentService = commentService;
            _cacheService = cacheService;
        }

        public async Task<CommentResponseDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync($"ad:{request.dto.AdId}", cancellationToken);
            await _cacheService.RemoveAsync($"adcomments:{request.dto.AdId}");

            return await _commentService.AddCommentAsync(request.dto, request.customerId);
        }

    }
}
