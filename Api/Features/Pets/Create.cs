using Api.Data;
using Api.Infrastructure.Validator;
using Core;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Pets
{
    public class Create
    {
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public string Breed { get; set; }
            public Guid OwnerId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(ApiDbContext db)
            {
                RuleFor(d => d.Name).NotEmpty();
                RuleFor(d => d.Breed).NotEmpty();
                RuleFor(d => d.OwnerId).NotEmpty().SetValidator(new UserExistenceValidator(db));
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Breed { get; set; }
            public Guid OwnerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ApiDbContext db;

            public Handler(ApiDbContext db)
            {
                this.db = db;
            }
            
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var pet = new Pet
                {
                    Name = request.Name,
                    Breed = request.Breed,
                    OwnerId = request.OwnerId,
                    InDonation = false,
                };

                db.Pets.Add(pet);

                await db.SaveChangesAsync(cancellationToken);

                return new Result
                {
                    Id = pet.Id,
                    Breed = pet.Breed,
                    Name = pet.Name,
                    OwnerId = pet.OwnerId
                };
            }
        }
    }
}
