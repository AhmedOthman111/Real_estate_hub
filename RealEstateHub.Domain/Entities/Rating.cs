using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Stars { get; set; } 
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }

        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        public String CustomerID { get; set; }

    }

}
