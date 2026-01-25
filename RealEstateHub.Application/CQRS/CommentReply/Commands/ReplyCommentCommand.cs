using MediatR;
using RealEstateHub.Application.CQRS.CommentReply.DTO;

namespace RealEstateHub.Application.CQRS.CommentReply.Commands
{
    public record ReplyCommentCommand(ReplyCommentDto dto, string AppUserownerId) : IRequest<ReplyResponseDto>;


}
