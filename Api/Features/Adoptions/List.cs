using Api.Data;
using Core.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Adoptions
{
    public class List
    {
        public class Query : IRequest<IEnumerable<Result>>
        {
            public DonationStatus? Status { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(d => d.Status).IsInEnum().When(d => d.Status.HasValue);
            }
        }

        public class Result
        {
            public Guid Id { get; set; }

            public PetResult Pet { get; set; }
            public UserResult User { get; set; }

            public class PetResult
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Breed { get; set; }
            }

            public class UserResult
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
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
                var query = db.Adoptions.Include(d => d.Pet)
                                        .Include(d => d.Adopter)
                                        .AsNoTracking();

                if (request.Status.HasValue)
                    query = query.Where(d => d.Status == request.Status);

                return await query.Select(d => new Result
                {
                    Id = d.Id,
                    Pet = d.Pet == null ? null : new Result.PetResult
                    {
                        Id = d.Pet.Id,
                        Name = d.Pet.Name,
                        Breed = d.Pet.Breed,
                    },
                    User = d.Adopter == null ? null : new Result.UserResult
                    {
                        Id = d.Adopter.Id,
                        Name = d.Adopter.Name
                    }
                }).ToListAsync(cancellationToken);
            }
        }
    }
}
