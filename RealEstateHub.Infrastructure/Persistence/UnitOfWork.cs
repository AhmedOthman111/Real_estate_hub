using RealEstateHub.Application.Interfaces;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IAdRepository Ad { get; }
        public IOwnerRepository Owner { get; }
        public ICategoryRepository Category { get; }
        public IAdPhotosRepository AdPhotosRepo { get; }
        public ICommentRepository CommentRepo { get; }
        public IReplyRepository ReplyRepo { get; }
        public IRatingRepository RatingRepo { get; }
        public ISaveAdRepository SaveAdRepo { get; }

        public UnitOfWork(ApplicationDbContext context, IAdRepository adRepository,
                            IOwnerRepository owner, ICategoryRepository category,
                            IAdPhotosRepository adPhotosRepo, ICommentRepository commentRepo,
                            IReplyRepository replyRepo, IRatingRepository ratingRepo
                            , ISaveAdRepository saveAdRepo)
        {
            _context = context;
            Ad = adRepository;
            Owner = owner;
            Category = category;
            AdPhotosRepo = adPhotosRepo;
            CommentRepo = commentRepo;
            ReplyRepo = replyRepo;
            RatingRepo = ratingRepo;
            SaveAdRepo = saveAdRepo;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
