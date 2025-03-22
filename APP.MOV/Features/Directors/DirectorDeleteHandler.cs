using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.MOV.Features.Directors
{
    public class DirectorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class DirectorDeleteHandler : MovieDBHandler, IRequestHandler<DirectorDeleteRequest, CommandResponse>
    {
        public DirectorDeleteHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DirectorDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Directors.Include(d => d.Movies).SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return Error("Director not found!");
            }

            if (entity.Movies.Any())
            {
                return Error("Director can't be deleted because they have related movies!");
            }

            _db.Directors.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Director deleted successfully", entity.Id);
        }
    }
}
