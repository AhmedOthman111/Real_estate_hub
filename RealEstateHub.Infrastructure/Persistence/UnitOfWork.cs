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

        public UnitOfWork(ApplicationDbContext context, IAdRepository adRepository, IOwnerRepository owner, ICategoryRepository category, IAdPhotosRepository adPhotosRepo)
        {
            _context = context;
            Ad = adRepository;
            Owner = owner;
            Category = category;
            AdPhotosRepo = adPhotosRepo;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
