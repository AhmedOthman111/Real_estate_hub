using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.Commands;
using RealEstateHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.CommentReply.Handlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly ICommentReplyService _commentReplyService;
        public DeleteCommentCommandHandler(ICommentReplyService commentReplyService) 
        {
            _commentReplyService = commentReplyService;
        }

        public async Task Handle(DeleteCommentCommand request  , CancellationToken cancellationToken) 
        {
            await _commentReplyService.DeleteCommentAsync(request.id, request.customerId);
        }
    }
}
