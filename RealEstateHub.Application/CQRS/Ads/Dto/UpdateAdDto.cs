using RealEstateHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Dto
{
    public class UpdateAdDto
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
        public int CategoryId { get; set; }
    }
}
