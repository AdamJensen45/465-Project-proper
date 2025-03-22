using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APP.MOV.Features.Genres
{
    public class GenreCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }

    public class GenreCreateHandler : MovieDBHandler, IRequestHandler<GenreCreateRequest, CommandResponse>
    {
        public GenreCreateHandler(MovieDB db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(GenreCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Genres.AnyAsync(g => g.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
            {
                return Error("Genre with the same name already exists!");
            }

            var entity = new Genre()
            {
                Name = request.Name.Trim()
            };

            _db.Genres.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Genre created successfully", entity.Id);
        }
    }
}
