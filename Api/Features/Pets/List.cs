using Api.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Pets
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Result>>
        {
            public string Search { get; set; }
            public Guid? OwnerId { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Breed { get; set; }
            public bool InDonation { get; set; }
            public OwnerResult Owner { get; set; }

            public class OwnerResult
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Email { get; set; }
            }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Result>>
        {
            private readonly ApiDbContext db;

            public Handler(ApiDbContext db)
            {
                this.db = db;
            }

            public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = db.Pets.Include(d => d.Owner).AsNoTracking();

                if (request.OwnerId.HasValue)
                    query = query.Where(d => d.OwnerId == request.OwnerId);

                if (!string.IsNullOrWhiteSpace(request.Search))
                    query = query.Where(d => d.Name.Contains(request.Search)
                                          || d.Breed.Contains(request.Search));

                return await query.Select(d => new Result
                {
                    Id = d.Id,
                    Name = d.Name,
                    Breed = d.Breed,
                    InDonation = d.InDonation,
                    Owner = d.Owner == null ? null : new Result.OwnerResult
                    {
                        Id = d.Owner.Id,
                        Name = d.Owner.Name,
                        Email = d.Owner.Email
                    }
                }).ToListAsync(cancellationToken);
            }
        }
    }
}
