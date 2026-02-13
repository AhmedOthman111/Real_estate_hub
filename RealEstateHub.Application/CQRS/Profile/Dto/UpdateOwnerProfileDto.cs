using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Profile.Dto
{
    public class UpdateOwnerProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string WhatsappNumber { get; set; }
        public string Bio { get; set; }
        public string CompanyName { get; set; }
    }
}
