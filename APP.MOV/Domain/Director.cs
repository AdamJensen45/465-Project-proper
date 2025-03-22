using CORE.APP.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APP.MOV.Domain
{
    public class Director : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        public bool IsRetired { get; set; }

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}