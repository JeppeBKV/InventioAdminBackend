using System.Net;
using InventioAdminBackend.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace InventioAdminBackend.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> CustomerList()
        {
            var response = await CosmosHelpers.RetrieveCustomerItemsAsync();
            if(response.Item2 == "No Errors") return Ok(response.Item1);
            return BadRequest(response.Item2);
        }
    }
}