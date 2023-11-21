using Microsoft.AspNetCore.Mvc;
using InventioAdminBackend.Models;
using InventioAdminBackend.Helpers;
using System.Text;
using System.Net;

namespace InventioAdminBackend.Controllers
{
    [ApiController]
    [Route("inventiouser")]
    public class InventiouserController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateModel _ctx)
        {
            var response = await InventioUserHelpers.CreateUser(_ctx);
            if(!response.Item1) return BadRequest(response.Item2);
            return Ok(response.Item2);
        }
    }
}