using CORE.APP.Features;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.MOV.Domain;
using Microsoft.EntityFrameworkCore;

namespace APP.MOV.Features.Movies
{
    public class MovieDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class MovieDeleteHandler : MovieDBHandler, IRequestHandler<MovieDeleteRequest, CommandResponse>
    {
        public MovieDeleteHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(MovieDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Movies
                .Include(m => m.MovieGenres)
                .SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return Error("Movie not found");
            }

            // Remove genre relationships first
            _db.MovieGenres.RemoveRange(entity.MovieGenres);

            // Then remove the movie
            _db.Movies.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Movie deleted successfully", entity.Id);
        }
    }
}