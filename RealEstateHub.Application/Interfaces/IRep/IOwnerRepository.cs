using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Interfaces.IRep
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        Task<Owner?> GetByAppUserIdAsync(string appUserId);
    }
}
