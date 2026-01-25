using RealEstateHub.Application.CQRS.CommentReply.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Ads.Dto
{
    public class AdWithAllDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double AreaSize { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Purpose { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string CategoryName { get; set; }
        public List<string> PhotoUrls { get; set; }
        public List<CommentResponseDto> Comments { get; set; } 

    }
}
