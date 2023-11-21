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
        public async Task<IActionResult> Create([FromBody] UsernamePasswordModel _ctx)
        {
            var response = await InventioUserHelpers.CreateUser(_ctx);
            if(!response.Item1) return BadRequest(response.Item2);
            return Ok(response.Item2);
        }
        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] UsernamePasswordModel _ctx)
        {
            var response = await CosmosHelpers.RetrieveUserItemAsync(_ctx.UserName);
            var response2 = await InventioUserHelpers.ValidateUser(response, _ctx.Password);
            if(response2) return Ok("Passwords match");
            return BadRequest("Wrong password");
        }
    }
}