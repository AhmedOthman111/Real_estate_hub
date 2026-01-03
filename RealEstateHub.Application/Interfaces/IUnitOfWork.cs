using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateHub.Application.Interfaces.IRep;

namespace RealEstateHub.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAdRepository Ad { get; }
        IOwnerRepository Owner { get; }
        ICategoryRepository Category { get; }

        IAdPhotosRepository AdPhotosRepo { get; }
        Task SaveChangesAsync();

    }
}
