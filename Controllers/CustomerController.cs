using System.Net;
using System.Text.Json;
using InventioAdminBackend.Helpers;
using InventioAdminBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Abstractions;
using Newtonsoft.Json;

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
        public async Task<IActionResult> UpdateCustomer([FromBody]CustomerInfo CustomerData, [FromHeader]string userId)
        {
            var response = await CosmosHelpers.EditCustomer(CustomerData);
            if(response != "Item Updated successfully") return BadRequest(response); 
            
            await LogHelpers.LogEditCustomer(userId, CustomerData);

            return Ok(response);
        }
        
        [HttpDelete("delete")] 
        public async Task<IActionResult> DeleteCustomer([FromBody] string Id, [FromHeader]string userId)
        {
            var response = await CosmosHelpers.DeleteCustomerAsync(Id);
            if(response != "Deleted successfully") return BadRequest("Error");
            
            await LogHelpers.LogDeleteCustomer(userId, Id);

            return Ok($"Customer {Id} deleted");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerInfo customer, [FromHeader]string userId)
        {
            var response = await CustomerHelpers.CreateUser(customer);
            if(!response.Item1) return BadRequest(response.Item2);
            
            await LogHelpers.LogCreateCustomer(userId, response.Item2);
            
            return Ok(response.Item2);

        }
    }
}