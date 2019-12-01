using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features
{
    [Route("api/[controller]")]
    public class DevController : ControllerBase
    {
        public DevController() { }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await Task.CompletedTask;

            return Ok("Teste");
        }
    }
}
