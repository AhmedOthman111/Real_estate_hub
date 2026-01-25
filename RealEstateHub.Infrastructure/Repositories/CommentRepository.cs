using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext) { }


        public async Task<List<Comment>> GetCommentsWithReplies(int adId)
        {
            return await _dbSet
                .Where(c => c.AdId == adId)
                .Include(c => c.Reply)
                .ThenInclude(o => o.Owner)
                .AsNoTracking()
                .ToListAsync();
        }




    }
}
