using CORE.APP.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APP.MOV.Domain
{
    public class Role : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation property for Users
        public List<User> Users { get; set; } = new List<User>();
    }
}