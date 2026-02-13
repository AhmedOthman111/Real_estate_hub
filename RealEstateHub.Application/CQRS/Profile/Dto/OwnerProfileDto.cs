using RealEstateHub.Application.CQRS.Rating.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Profile.Dto
{
    public class OwnerProfileDto
    {
        public string? Mail { get; set; }
        public string? UserName{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string WhatsappNumber { get; set; }
        public string Bio { get; set; }
        public string CompanyName { get; set; }
        public double AverageRating { get; set; }
        public List<RatingResponseDto> Ratings { get; set; }
    }
}
