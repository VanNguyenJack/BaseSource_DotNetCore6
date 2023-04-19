using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.DTOs.Identity
{
    public class TokenResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string JWToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
