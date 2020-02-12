using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;

        public DataInitializer(IUserService userService, IEventService eventService)
        {
            _userService = userService;
            _eventService = eventService;
        }

        public async Task SeedAsync()
        {
            var tasks = new List<Task>();
            tasks.Add(_userService.RegisterAsync(Guid.NewGuid(), "user@gmail.com", "default user", "secret"));
            tasks.Add(_userService.RegisterAsync(Guid.NewGuid(), "admin@gmail.com", "admin user", "secret", "admin"));

            for (int i = 1; i < 6; i++)
            {
                var eventId = Guid.NewGuid();
                var name = $"Event {i}";
                var desc = $"{name} description.";
                var startDate = DateTime.UtcNow.AddHours(5);
                var endDate = startDate.AddHours(3);

                tasks.Add(_eventService.CreateAsync(eventId, name, desc, startDate, endDate));
                tasks.Add(_eventService.AddTicketsAsync(eventId, 300, 90));
            }

            await Task.WhenAll(tasks);
        }
    }
}
