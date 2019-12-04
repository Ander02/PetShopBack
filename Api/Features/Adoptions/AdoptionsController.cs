using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Features.Adoptions
{
    public class AdoptionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AdoptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ChangeStatus.Command>> ChangeStatus([FromBody] ChangeStatus.Command request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<List.Result>>> List([FromBody] List.Query request)
            => Ok(await mediator.Send(request));
    }
}
