using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Users
{
    public class List
    {
        public class Query : IRequest<IEnumerable<UserResult>>
        {
            public string Search { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<UserResult>>
        {
            private readonly ApiDbContext db;

            public Handler(ApiDbContext db)
            {
                this.db = db;
            }

            public async Task<IEnumerable<UserResult>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = db.Users.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.Search))
                    query = query.Where(d => d.Name.Contains(request.Search));

                return await query.Select(d => new UserResult(d)).ToListAsync(cancellationToken);
            }
        }
    }
}
