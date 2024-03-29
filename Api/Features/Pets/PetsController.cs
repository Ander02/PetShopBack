﻿using Api.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Features.Pets
{
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PetsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UserResult>> Create([FromBody] Create.Command request)
            => Ok(await mediator.Send(request));

        [Authorize]
        [HttpPost("{id}/adopt")]
        public async Task<ActionResult<RegisterInterest.Result>> RegisterInterest([FromRoute] Guid id, [FromBody] RegisterInterest.Command request)
        {
            request.PetId = id;
            return Ok(await mediator.Send(request));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResult>>> List([FromQuery] List.Query request)
            => Ok(await mediator.Send(request));

    }
}
