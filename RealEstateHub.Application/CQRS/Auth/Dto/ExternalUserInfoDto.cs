using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Dto
{
    public class ExternalUserInfoDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
