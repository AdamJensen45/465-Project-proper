using CORE.APP.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.MOV.Domain
{
    public class Skill : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Collection of UserSkill relationships
        public List<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

        // Helper property to work with User IDs
        [NotMapped]
        public List<int> UserIds
        {
            get => UserSkills?.Select(us => us.UserId).ToList();
            set => UserSkills = value?.Select(v => new UserSkill() { UserId = v }).ToList();
        }
    }
}