using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Domain.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Address { get; set; }
        public string WhatsappNumber { get; set; }
        public string Bio { get; set; }
        public string CompanyName { get; set; }
        public double AverageRating { get; set; }
        public ICollection<Ad> ads { get; set; }
    }
}
