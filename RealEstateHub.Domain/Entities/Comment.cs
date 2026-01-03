using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerID { get; set; }

        public int AdId { get; set; }
        public Ad Ad { get; set; }

        public Reply Reply { get; set; }
    }

}
