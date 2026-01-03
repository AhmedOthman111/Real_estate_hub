using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Domain.Entities
{
    public class AdPhoto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; } = false;
        public int AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
