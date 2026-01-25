using RealEstateHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RealEstateHub.Domain.Entities
{
    public  class Ad
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public double AreaSize { get; set; }

        public string City { get; set; }      
        public string Area { get; set; }      
        public string Address { get; set; }   

        public AdPurpose Purpose { get; set; }
        public AdPriority Priority { get; set; }
        public AdStatus Status { get; set; } = AdStatus.Pending;

        public int DurationDays { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool ExpiryReminderSent { get; set; } = false;
        public bool ExpiredEmailSent { get; set; } = false;

        public DateTime ExpireAt { get; set; }

        // Relations
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<AdPhoto> Photos { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
