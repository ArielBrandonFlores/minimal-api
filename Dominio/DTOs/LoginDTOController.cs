using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.DTOs
{
    public class LoginDTOController
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}