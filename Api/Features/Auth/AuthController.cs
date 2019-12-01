using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Features.Auth
{
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Auth.Command>> Auth([FromBody] Auth.Command request)
        {
            return Ok(await mediator.Send(request));
        }
    }
}
