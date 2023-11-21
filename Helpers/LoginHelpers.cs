using System.Net;
using System.Text;
using System.Text.Json;
using InventioAdminBackend.Models;
using Microsoft.Azure.Cosmos;

namespace InventioAdminBackend.Helpers
{
    public class InventioUserHelpers
    {
        public static async Task<(bool, string)> CreateUser(CreateModel _ctx)
        {
            InventioLoginModel IITLoginModel = new("4", _ctx.UserName, _ctx.Password);
            string json = JsonSerializer.Serialize(IITLoginModel);
            var response = await CosmosHelpers.InsertIntoCosmos("InventioLogin", "userName", json);

            return response;
        }
    }
}