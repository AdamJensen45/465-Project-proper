using CORE.APP.Domain;

namespace APP.MOV.Domain
{
    public class MovieGenre : Entity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}