using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PetShop.Features.Users
{
    public class BaseFeature
    {
        public class Command : IRequest<Result>
        {

        }

        public class Validator
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
