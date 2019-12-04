using Api.Data;
using Api.Infrastructure.Notifications;
using Core.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Adoptions
{
    public class ChangeStatus
    {
        public class Command : IRequest<Result>
        {
            public Guid Id { get; set; }
            public DonationStatus Status { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(d => d.Id).NotEmpty();
                RuleFor(d => d.Status).IsInEnum();
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public DonationStatus Status { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ApiDbContext db;
            private readonly NotificationContext notificationContext;

            public Handler(ApiDbContext db, NotificationContext notificationContext)
            {
                this.db = db;
                this.notificationContext = notificationContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var adoption = await db.Adoptions.FirstOrDefaultAsync(d => d.Status == request.Status, cancellationToken);

                if (adoption == null)
                {
                    notificationContext.AddDefaultResourseNotFoundNotification();
                    return null;
                }

                adoption.Status = request.Status;

                await db.SaveChangesAsync();

                return new Result
                {
                    Id = adoption.Id,
                    Status = adoption.Status
                };
            }
        }
    }
}
