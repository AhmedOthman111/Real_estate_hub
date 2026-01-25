using RealEstateHub.Application.Interfaces.IRep;

namespace RealEstateHub.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAdRepository Ad { get; }
        IOwnerRepository Owner { get; }
        ICategoryRepository Category { get; }
        IAdPhotosRepository AdPhotosRepo { get; }
        ICommentRepository CommentRepo { get; }
        IReplyRepository ReplyRepo { get; }
        Task SaveChangesAsync();

    }
}
