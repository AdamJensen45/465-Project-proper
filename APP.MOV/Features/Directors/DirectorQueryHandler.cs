using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.MOV.Features.Directors
{
    public class DirectorQueryRequest : Request, IRequest<IQueryable<DirectorQueryResponse>>
    {
    }

    public class DirectorQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRetired { get; set; }
        public int MovieCount { get; set; }
    }

    public class DirectorQueryHandler : MovieDBHandler, IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
    {
        public DirectorQueryHandler(MovieDB db) : base(db)
        {
        }

        public Task<IQueryable<DirectorQueryResponse>> Handle(DirectorQueryRequest request, CancellationToken cancellationToken)
        {
            IQueryable<DirectorQueryResponse> query = _db.Directors
                .OrderBy(d => d.Name)
                .Select(d => new DirectorQueryResponse()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Surname = d.Surname,
                    IsRetired = d.IsRetired,
                    MovieCount = d.Movies.Count
                });

            return Task.FromResult(query);
        }
    }
}