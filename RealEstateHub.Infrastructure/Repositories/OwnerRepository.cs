using RealEstateHub.Application.Exceptions;
using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;
using RealEstateHub.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class OwnerRepository : GenericRepository<Owner> , IOwnerRepository
    {
        public OwnerRepository(ApplicationDbContext context) : base(context) { }


        public async Task<Owner> GetByAppUserIdAsync (string appuser)
        {

            var owner = _dbSet.FirstOrDefault(o => o.AppUserId == appuser);
            if (owner == null) throw new NotFoundException("Owner", appuser);
            return owner;
        }


    }
}
