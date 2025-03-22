using APP.MOV.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.MOV.Features.Directors
{
        public class DirectorCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(100)]
        public string Surname { get; set; }
        
        public bool IsRetired { get; set; }
    }
    
    public class DirectorCreateHandler : MovieDBHandler, IRequestHandler<DirectorCreateRequest, CommandResponse>
    {
        public DirectorCreateHandler(MovieDB db) : base(db)
        {
        }
        
        public async Task<CommandResponse> Handle(DirectorCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Directors.AnyAsync(d => 
                d.Name.ToUpper() == request.Name.ToUpper().Trim() && 
                d.Surname.ToUpper() == (request.Surname.ToUpper().Trim() ?? ""), 
                cancellationToken))
            {
                return Error("Director with the same name and surname already exists!");
            }
            
            var entity = new Domain.Director()
            {
                Name = request.Name.Trim(),
                Surname = request.Surname?.Trim(),
                IsRetired = request.IsRetired
            };
            
            _db.Directors.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);
            
            return Success("Director created successfully", entity.Id);
        }
    }

}