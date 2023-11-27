using System.Net;
using InventioAdminBackend.Helpers;
using InventioAdminBackend.Models;
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

        [HttpPost("edit")]
        public async Task<IActionResult> UpdateCustomer([FromBody]CustomerInfo CustomerData)
        {
            var response = await CosmosHelpers.EditCustomer(CustomerData);
            if(response == "Item Updated successfully") return Ok(response);
            return BadRequest(response);
        }
    }
}