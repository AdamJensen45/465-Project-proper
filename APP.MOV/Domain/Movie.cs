using CORE.APP.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.MOV.Domain
{
    public class Movie : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        [Required]
        public int DirectorId { get; set; }  

        public Director Director { get; set; }

        public List<MovieGenre> MovieGenres { get; private set; } = new List<MovieGenre>();

        [NotMapped]
        public List<int> GenreIds
        {
            get => MovieGenres?.Select(mg => mg.GenreId).ToList();
            set => MovieGenres = value?.Select(v => new MovieGenre() { GenreId = v }).ToList();
        }
    }
}