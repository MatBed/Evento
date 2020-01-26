using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Core.Domain
{
    public class Ticket : Entity
    {
        public Guid EventId { get; set; }
        public int Seating { get; set; }
        public decimal Price { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? PurchasedAt { get; set; }
        public bool Purchased => UserId.HasValue;

        protected Ticket() { }

        public Ticket(Event @event, int seating, decimal price)
        {
            EventId = @event.Id;
            Seating = seating;
            Price = price;
        }
    }
}
