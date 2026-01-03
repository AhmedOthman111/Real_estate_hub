using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Dto
{
    public class LoginDto
    {
        public  string identity { get; set; }
        public string Password { get; set; }
    }
}
