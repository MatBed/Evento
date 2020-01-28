using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.Commands.Events
{
    public class UpdateEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
