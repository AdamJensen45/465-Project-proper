using APP.MOV.Domain;
using APP.MOV.Features.Directors;
using APP.MOV.Features.Genres;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace APP.MOV.Features.Movies
{
    public class MovieQueryRequest : Request, IRequest<IQueryable<MovieQueryResponse>>
    {
        public string Name { get; set; }
        public int? DirectorId { get; set; }
        public List<int> GenreIds { get; set; }
        public DateTime? ReleaseDateStart { get; set; }
        public DateTime? ReleaseDateEnd { get; set; }
        public decimal? TotalRevenueMin { get; set; }
        public decimal? TotalRevenueMax { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

    public class MovieQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ReleaseDateFormatted { get; set; }
        public decimal TotalRevenue { get; set; }
        public string TotalRevenueFormatted { get; set; }

        public DirectorQueryResponse Director { get; set; }

        public List<int> GenreIds { get; set; }
        public string GenreNames { get; set; }
        public List<GenreQueryResponse> Genres { get; set; }
    }

    public class MovieQueryHandler : MovieDBHandler, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
    {
        public MovieQueryHandler(MovieDB db) : base(db)
        {
        }

        public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = _db.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .OrderBy(m => m.Name)
                .AsQueryable();

            // Apply filters if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
                entityQuery = entityQuery.Where(m => m.Name.ToUpper().Contains(request.Name.ToUpper().Trim()));

            if (request.DirectorId.HasValue)
                entityQuery = entityQuery.Where(m => m.DirectorId == request.DirectorId.Value);

            if (request.GenreIds != null && request.GenreIds.Any())
                entityQuery = entityQuery.Where(m => m.MovieGenres.Any(mg => request.GenreIds.Contains(mg.GenreId)));

            if (request.ReleaseDateStart.HasValue)
                entityQuery = entityQuery.Where(m => m.ReleaseDate >= request.ReleaseDateStart.Value);

            if (request.ReleaseDateEnd.HasValue)
                entityQuery = entityQuery.Where(m => m.ReleaseDate <= request.ReleaseDateEnd.Value);

            if (request.TotalRevenueMin.HasValue)
                entityQuery = entityQuery.Where(m => m.TotalRevenue >= request.TotalRevenueMin.Value);

            if (request.TotalRevenueMax.HasValue)
                entityQuery = entityQuery.Where(m => m.TotalRevenue <= request.TotalRevenueMax.Value);

            var query = entityQuery.Select(m => new MovieQueryResponse()
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                ReleaseDateFormatted = m.ReleaseDate.HasValue ? m.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                TotalRevenue = m.TotalRevenue,
                TotalRevenueFormatted = m.TotalRevenue.ToString("C"),

                GenreIds = m.GenreIds,
                GenreNames = string.Join(", ", m.MovieGenres.Select(mg => mg.Genre.Name)),

                Director = m.Director == null ? null : new DirectorQueryResponse
                {
                    Id = m.Director.Id,
                    Name = m.Director.Name,
                    Surname = m.Director.Surname,
                    IsRetired = m.Director.IsRetired
                },

                Genres = m.MovieGenres.Select(mg => new GenreQueryResponse
                {
                    Id = mg.Genre.Id,
                    Name = mg.Genre.Name
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }
}