using RealEstateHub.Application.Interfaces.IRep;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.Repositories
{
    public class AdPhotosRepository : GenericRepository<AdPhoto>, IAdPhotosRepository
    {
        public AdPhotosRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
