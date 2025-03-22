using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using CORE.APP.Features;
using System.ComponentModel.DataAnnotations;
using APP.MOV.Domain;
using Microsoft.EntityFrameworkCore;

namespace APP.MOV.Features.Movies
{
    public class MovieUpdateRequest : Request, IRequest<CommandResponse>
    {
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

    public class MovieUpdateHandler : MovieDBHandler, IRequestHandler<MovieUpdateRequest, CommandResponse>
    {
        public MovieUpdateHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
        {
            // Check if another movie with same name exists
            if (await _db.Movies.AnyAsync(t =>
                t.Id != request.Id &&
                t.Name.ToUpper() == request.Name.ToUpper().Trim(),
                cancellationToken))
            {
                return Error("Another movie with the same name already exists!");
            }

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

            var entity = await _db.Movies
                .Include(m => m.MovieGenres)
                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return Error("Movie not found");
            }

            // Remove existing genre relationships
            _db.MovieGenres.RemoveRange(entity.MovieGenres);

            // Update properties
            entity.Name = request.Name.Trim();
            entity.ReleaseDate = request.ReleaseDate;
            entity.TotalRevenue = request.TotalRevenue;
            entity.DirectorId = request.DirectorId;
            entity.GenreIds = request.GenreIds;

            _db.Movies.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Movie updated successfully");
        }
    }
}