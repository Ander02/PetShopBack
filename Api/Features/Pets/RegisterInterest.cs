using Api.Data;
using Api.Infrastructure.Validator;
using Core;
using Core.Enums;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Pets
{
    public class RegisterInterest
    {
        public class Command : IRequest<Result>
        {
            public Guid PetId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(ApiDbContext db)
            {
                RuleFor(d => d.PetId).NotEmpty().SetValidator(new PetExistenceValidator(db));
                RuleFor(d => d.UserId).NotEmpty().SetValidator(new UserExistenceValidator(db));
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public Guid PetId { get; set; }
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
                var adoption = new Adoption
                {
                    AdopterId = request.UserId,
                    PetId = request.PetId,
                    Status = DonationStatus.HasInterest
                };

                db.Adoptions.Add(adoption);

                await db.SaveChangesAsync(cancellationToken);

                return new Result
                {
                    Id = adoption.Id,
                    UserId = adoption.AdopterId,
                    PetId = adoption.PetId
                };
            }
        }
    }
}
