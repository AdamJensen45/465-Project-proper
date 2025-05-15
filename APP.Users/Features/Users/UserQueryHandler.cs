using APP.Users.Domain;
using APP.Users.Features.Roles;
using APP.Users.Features.Skills;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : QueryResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Active { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }

        // Way 1:
        public string RoleName { get; set; }

        // Way 2:
        public RoleQueryResponse Role { get; set; }

        public List<int> SkillIds { get; set; }

        // Way 1:
        public string SkillNames { get; set; }

        // Way 2:
        public List<SkillQueryResponse> Skills { get; set; }
    }

    public class UserQueryHandler : UsersDbHandler, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(UsersDb db) : base(db)
        {
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = _db.Users.Include(u => u.Role).Include(u => u.UserSkills).ThenInclude(us => us.Skill)
                .OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).AsQueryable();

            // projection:
            var query = entityQuery.Select(u => new UserQueryResponse()
            {
                Active = u.IsActive ? "Active" : "Not Active",
                FirstName = u.FirstName,
                FullName = u.FirstName + " " + u.LastName,
                Id = u.Id,
                IsActive = u.IsActive,
                LastName = u.LastName,
                UserName = u.UserName,
                Password = u.Password,
                RoleId = u.Role.Id,

                // Way 1:
                RoleName = u.Role.Name,

                // Way 2:
                Role = new RoleQueryResponse()
                {
                    Name = u.Role.Name
                },

                SkillIds = u.SkillIds,

                // Way 1:
                SkillNames = string.Join(", ", u.UserSkills.Select(us => us.Skill.Name)),

                // Way 2:
                Skills = u.UserSkills.Select(us => new SkillQueryResponse()
                {
                    Name = us.Skill.Name
                }).ToList()
            });

            return Task.FromResult(query); 
        }
    }
}
