using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.MOV.Features.Genres
{
    public class GenreDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class GenreDeleteHandler : MovieDBHandler, IRequestHandler<GenreDeleteRequest, CommandResponse>
    {
        public GenreDeleteHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(GenreDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Genres.Include(g => g.MovieGenres).SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return Error("Genre not found!");
            }

            if (entity.MovieGenres.Any())
            {
                return Error("Genre can't be deleted because it's associated with movies!");
            }

            _db.Genres.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Genre deleted successfully", entity.Id);
        }
    }
}

