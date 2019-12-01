using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Users
{
    public class BaseFeature
    {
        public class Command : IRequest<Result>
        {

        }

        public class Validator : AbstractValidator<Command>
        {

        }

        public class Result
        {

        }

        public class Handler : IRequestHandler<Command, Result>
        {
            public Handler()
            {
            }

            public Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
