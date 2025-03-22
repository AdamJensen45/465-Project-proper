using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;

namespace APP.MOV.Features.Genres
{
    public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>>
    {
    }

    public class GenreQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public int MovieCount { get; set; }
    }

    public class GenreQueryHandler : MovieDBHandler, IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        public GenreQueryHandler(MovieDB db) : base(db)
        {
        }

        public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request, CancellationToken cancellationToken)
        {
            IQueryable<GenreQueryResponse> query = _db.Genres
                .OrderBy(g => g.Name)
                .Select(g => new GenreQueryResponse()
                {
                    Id = g.Id,
                    Name = g.Name,
                    MovieCount = g.MovieGenres.Count
                });

            return Task.FromResult(query);
        }
    }
}