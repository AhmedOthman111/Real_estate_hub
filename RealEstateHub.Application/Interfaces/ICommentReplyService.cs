using RealEstateHub.Application.CQRS.CommentReply.DTO;

namespace RealEstateHub.Application.Interfaces
{
    public interface ICommentReplyService
    {
        Task<CommentResponseDto> AddCommentAsync(CreateCommentDto dto, string customerId);
        Task<ReplyResponseDto> ReplyToCommentAsync(ReplyCommentDto dto, string AppUserownerId);
        Task DeleteCommentAsync(int id, string customerId);
        Task DeleteReplyAsync(int id, string AppUserownerId);
        Task<List<CommentResponseDto>> GetCommentsByAdIdAsync(int adId);


    }
}
