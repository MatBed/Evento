using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.DTO
{
    public class TokenDto : JwtDto
    {
        public string Role { get; set; }
    }
}
