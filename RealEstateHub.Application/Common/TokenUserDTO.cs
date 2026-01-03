using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Application.Common
{
    public class TokenUserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}
