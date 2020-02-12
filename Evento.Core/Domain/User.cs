using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Evento.Core.Domain
{
    public class User : Entity
    {
        public string Role { get; protected set; }
        
        [Required(ErrorMessage = "User can not have an empty name")]
        public string Name { get; protected set; }

        [EmailAddress]
        [Required(ErrorMessage = "User can not have an empty email")]
        public string Email { get; protected set; }
        
        [Required(ErrorMessage = "User can not have an empty password")]
        public string Password { get; protected set; }
        
        public DateTime CreatedAt { get; protected set; }

        protected User()
        {

        }

        public User(Guid id, string role,string name,
            string email, string password)
        {
            Id = id;
            Role = role;
            Name = name;
            Email = email;
            Password = password;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
