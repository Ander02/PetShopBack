using Api.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Features.Users
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UserResult>> Create([FromBody] Create.Command request)
            => Ok(await mediator.Send(request));

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResult>>> List([FromBody] List.Query request)
            => Ok(await mediator.Send(request));

    }
}
