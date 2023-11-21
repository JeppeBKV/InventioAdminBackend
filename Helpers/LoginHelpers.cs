using System.Net;
using System.Text;
using System.Text.Json;
using InventioAdminBackend.Models;
using Microsoft.Azure.Cosmos;

namespace InventioAdminBackend.Helpers
{
    public class InventioUserHelpers
    {
        public static async Task<(bool, string)> CreateUser(UsernamePasswordModel _ctx)
        {
            string newGuid = Guid.NewGuid().ToString();
            InventioLoginModel IITLoginModel = new(newGuid, _ctx.UserName, _ctx.Password);
            string json = JsonSerializer.Serialize(IITLoginModel);
            var response = await CosmosHelpers.InsertIntoCosmos("InventioLoginV2", "UserName", json);

            return response;
        }

        public static async Task<bool> ValidateUser(string dbpassword, string passwordSubmitted)
        {
            if(dbpassword == passwordSubmitted) return true;
            return false;
        }
    }
}