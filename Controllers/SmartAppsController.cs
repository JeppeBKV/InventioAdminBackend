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

        [HttpGet("update/smartapps")]
        public async Task<IActionResult> GetSmartapps()
        {
            var response = await CosmosHelpers.RetrieveCustomerSmartApps();
            var response2 = await SmartAppsHelpers.CountInstalledSmartApps(response);
            var response3 = await CosmosHelpers.EditSmartApps(response2);

            return Ok(response3);
        }
    }
}