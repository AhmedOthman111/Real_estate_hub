using RealEstateHub.Application.CQRS.Ads.Dto;
using RealEstateHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.SaveAd.Dto
{
    public class SaveAdResponseDto
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public string CategoryName { get; set; }
        public string AdPurpose { get; set; }
        public string AdTitle { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
