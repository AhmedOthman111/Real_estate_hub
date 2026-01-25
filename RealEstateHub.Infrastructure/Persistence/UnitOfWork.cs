using RealEstateHub.Application.Interfaces;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public UnitOfWork(ApplicationDbContext context, IAdRepository adRepository, IOwnerRepository owner, ICategoryRepository category, IAdPhotosRepository adPhotosRepo , ICommentRepository commentRepo, IReplyRepository replyRepo)
        {
            _context = context;
            Ad = adRepository;
            Owner = owner;
            Category = category;
            AdPhotosRepo = adPhotosRepo;
            CommentRepo = commentRepo;
            ReplyRepo = replyRepo;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
