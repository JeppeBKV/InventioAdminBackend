using Microsoft.AspNetCore.Mvc;

namespace InventioAdminBackend.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> CustomerList([FromQuery] string indentifier)
        {
            
            return Ok();
        }
    }
}