using Microsoft.AspNetCore.Mvc;
using InventioAdminBackend.Helpers;
using InventioAdminBackend.Models;

namespace InventioAdminBackend.Controllers
{
    [ApiController]
    [Route("api/smartapps")]
    public class SMARTappsController : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var response = await CosmosHelpers.RetrieveSmartAppsAsync(); 
            if (response.Item2 == "No Errors") return Ok(response.Item1);
            return BadRequest(response.Item2);
        }
    }
}