using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Rating.Dto
{
    public class CreateRatingDto
    {
        public int OwnerId { get; set; }
        public int Stars { get; set; }
        public string Review { get; set; }
    }
}
