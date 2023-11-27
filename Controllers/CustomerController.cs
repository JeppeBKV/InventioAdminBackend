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
        
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCustomer([FromBody] string customerId)
        {
            var response = await CosmosHelpers.DeleteCustomerAsync(customerId);
            if(response == "Deleted successfully") return Ok($"Customer {customerId} deleted");
            return BadRequest("Error");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerInfo customer)
        {
            var response = await CustomerHelpers.CreateUser(customer);
            if(!response.Item1) return BadRequest(response.Item2);
            return Ok(response.Item2);

        }
    }
}