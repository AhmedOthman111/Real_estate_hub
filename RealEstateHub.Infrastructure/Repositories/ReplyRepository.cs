using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        public ReplyRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Reply> GetReplyWithComment(int replyId)
        {
            var reply = await _dbSet
                               .Include(r => r.Comment)
                               .FirstOrDefaultAsync(r => r.Id == replyId);

            if (reply == null) throw new NotFoundException("reply", replyId);

            return reply;
        }



    }
}
