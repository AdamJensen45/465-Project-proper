using CORE.APP.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.MOV.Domain
{
    public class Genre : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

        [NotMapped]
        public List<int> MovieIds
        {
            get => MovieGenres?.Select(mg => mg.MovieId).ToList();
            set => MovieGenres = value?.Select(v => new MovieGenre() { MovieId = v }).ToList();
        }
    }
}