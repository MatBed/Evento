using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.Commands.Users
{
    public class Register
    {
        public Guid UserId { get; set; }
        public string Role { get; protected set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
    }
}
