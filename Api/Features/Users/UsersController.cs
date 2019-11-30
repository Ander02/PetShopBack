using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PetShop.Features.Users
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult<Create.Result>> Create([FromBody] Create.Command request) 
            => Ok(await mediator.Send(request));
    }
}
