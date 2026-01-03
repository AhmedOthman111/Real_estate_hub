using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Dto
{
    public class RegisterOwnerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }
        public string WhatsappNumber { get; set; }
        public string Bio { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
