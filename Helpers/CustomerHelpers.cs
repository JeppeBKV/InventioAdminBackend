using InventioAdminBackend.Models;
using System.Text.Json;

namespace InventioAdminBackend.Helpers
{
    public class CustomerHelpers
    {
        public static async Task<(bool, string)> CreateUser(CustomerInfo _ctx)
        {
            string newGuid = Guid.NewGuid().ToString();

            // CustomerInfo CustomerInfo = new(newGuid, _ctx.UserName, _ctx.Password);
            CustomerInfo customerInfo = _ctx;
            customerInfo.id = newGuid;
            customerInfo.Created = DateTime.UtcNow;
            string json = JsonSerializer.Serialize(customerInfo);
            var response = await CosmosHelpers.InsertIntoCosmos("InventioKunderV2", "id", json);

            return response;
        }
        
    }
}