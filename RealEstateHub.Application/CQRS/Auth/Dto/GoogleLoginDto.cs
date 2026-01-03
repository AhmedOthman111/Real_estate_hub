using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.CQRS.Auth.Dto
{
    public class GoogleLoginDto
    {
        public string IdToken { get; set; } = string.Empty;

        public string role {  get; set; }   

    }
}
