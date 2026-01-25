using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.Commands;
using RealEstateHub.Application.CQRS.CommentReply.DTO;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.CommentReply.Handlers
{
    public class ReplyCommentCommandHandler : IRequestHandler<ReplyCommentCommand, ReplyResponseDto>
    {
        private readonly ICommentReplyService _commentReplyService;
        public ReplyCommentCommandHandler(ICommentReplyService commentReply)
        {
            _commentReplyService = commentReply;
        }

        public async Task<ReplyResponseDto> Handle(ReplyCommentCommand request, CancellationToken cancellationToken)
        {
            return await _commentReplyService.ReplyToCommentAsync(request.dto, request.AppUserownerId);
        }
    }
}
