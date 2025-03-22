using System.ComponentModel.DataAnnotations;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using APP.MOV.Domain;
using System.Text.Json.Serialization;

namespace APP.MOV.Features.Movies
{
    public class MovieCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        [Required]
        public int DirectorId { get; set; }

        public List<int> GenreIds { get; set; }
    }

    class MovieCreateHandler : MovieDBHandler, IRequestHandler<MovieCreateRequest, CommandResponse>
    {
        public MovieCreateHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            // Check if movie with same name already exists
            if (await _db.Movies.AnyAsync(t => t.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Movie with the same name already exists!");

            // Check if director exists
            if (!await _db.Directors.AnyAsync(d => d.Id == request.DirectorId, cancellationToken))
                return Error("Selected director does not exist!");

            // Check if all genres exist
            if (request.GenreIds != null && request.GenreIds.Any())
            {
                var existingGenreIds = await _db.Genres
                    .Where(g => request.GenreIds.Contains(g.Id))
                    .Select(g => g.Id)
                    .ToListAsync(cancellationToken);

                if (existingGenreIds.Count != request.GenreIds.Count)
                    return Error("One or more selected genres do not exist!");
            }

            var entity = new Movie()
            {
                Name = request.Name.Trim(),
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId,
                GenreIds = request.GenreIds
            };

            _db.Movies.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Movie created successfully", entity.Id);
        }
    }
}