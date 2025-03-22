using CORE.APP.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APP.MOV.Domain
{
    public class User : Entity
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        // Collection of UserSkill relationships
        public List<UserSkill> UserSkills { get; private set; } = new List<UserSkill>();
    }
}