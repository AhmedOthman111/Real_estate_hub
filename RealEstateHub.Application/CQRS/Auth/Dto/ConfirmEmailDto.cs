using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Dto
{
    public class ConfirmEmailDto
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
    }
}
