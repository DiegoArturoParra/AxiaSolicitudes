using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttentionAxia.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{Name} {LastName}";
        public int RolId { get; set; }
        public string Rol { get; set; }
    }
}