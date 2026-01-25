using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Application.Interfaces.IRep
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsWithReplies(int adId);
    }
}
