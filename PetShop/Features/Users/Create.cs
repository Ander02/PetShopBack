using Core;
using Core.Enums;
using FluentValidation;
using MediatR;
using PetShop.Data;
using PetShop.Infrastructure.Validator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PetShop.Features.Users
{
    public class Create
    {
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public PersonType PersonType { get; set; }
            public string LegalDocument { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(d => d.Name).NotEmpty();
                RuleFor(d => d.Email).NotEmpty().EmailAddress();
                RuleFor(d => d.PersonType).IsInEnum();
                RuleFor(d => d.PhoneNumber).NotEmpty();
                RuleFor(d => d.Password).NotEmpty().MinimumLength(6);

                When(d => d.PersonType == PersonType.Legal, () =>
                {
                    RuleFor(d => d.LegalDocument).NotEmpty().SetValidator(new CnpjValidator());
                }).Otherwise(() =>
                {
                    RuleFor(d => d.LegalDocument).NotEmpty().SetValidator(new CpfValidator());
                });
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public PersonType PersonType { get; set; }
            public string LegalDocument { get; set; }
            public string PhoneNumber { get; set; }
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
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PersonType = request.PersonType,
                    PhoneNumber = request.PhoneNumber,
                };

                user.SetPassword(request.Password);

                db.Users.Add(user);

                await db.SaveChangesAsync(cancellationToken);

                return new Result
                {
                    Id = user.Id,
                    Email = user.Email,
                    LegalDocument = user.LegalDocument,
                    Name = user.Name,
                    PersonType = user.PersonType,
                    PhoneNumber = user.PhoneNumber
                };
            }
        }
    }
}
