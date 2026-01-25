using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.Commands;
using RealEstateHub.Application.Interfaces;

namespace RealEstateHub.Application.CQRS.CommentReply.Handlers
{
    public class DeleteReplyCommandHandler : IRequestHandler<DeleteReplyCommand>
    {
        private readonly ICommentReplyService _commentReplyService;
        public DeleteReplyCommandHandler(ICommentReplyService commentReplyService)
        {
            _commentReplyService = commentReplyService;
        }

        public async Task Handle(DeleteReplyCommand request, CancellationToken cancellationToken)
        {
            await _commentReplyService.DeleteReplyAsync(request.id, request.AppUserownerId);
        }
    }
}
