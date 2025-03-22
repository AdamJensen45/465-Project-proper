using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.MOV.Features.Directors
{
    public class DirectorUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        public bool IsRetired { get; set; }
    }

    public class DirectorUpdateHandler : MovieDBHandler, IRequestHandler<DirectorUpdateRequest, CommandResponse>
    {
        public DirectorUpdateHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DirectorUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Directors.AnyAsync(d =>
                d.Id != request.Id &&
                d.Name.ToUpper() == request.Name.ToUpper().Trim() &&
                d.Surname.ToUpper() == (request.Surname.ToUpper().Trim() ?? ""),
                cancellationToken))
            {
                return Error("Another director with the same name and surname already exists!");
            }

            var entity = await _db.Directors.FindAsync(request.Id, cancellationToken);

            if (entity is null)
            {
                return Error("Director not found!");
            }

            entity.Name = request.Name.Trim();
            entity.Surname = request.Surname?.Trim();
            entity.IsRetired = request.IsRetired;

            _db.Directors.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Director updated successfully", entity.Id);
        }
    }
}