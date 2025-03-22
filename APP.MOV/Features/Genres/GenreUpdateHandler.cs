using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.MOV.Features.Genres
{
    public class GenreUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }

    public class GenreUpdateHandler : MovieDBHandler, IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        public GenreUpdateHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(GenreUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Genres.AnyAsync(g =>
                g.Id != request.Id &&
                g.Name.ToUpper() == request.Name.ToUpper().Trim(),
                cancellationToken))
            {
                return Error("Another genre with the same name already exists!");
            }

            var entity = await _db.Genres.FindAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                return Error("Genre not found!");
            }

            entity.Name = request.Name.Trim();

            _db.Genres.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Genre updated successfully", entity.Id);
        }
    }

}
