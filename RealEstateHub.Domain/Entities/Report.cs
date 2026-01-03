using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }

        public int AdId { get; set; }
        public Ad Ad { get; set; }

        public string CustomerId { get; set; }
    }

}
