using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InventioAdminBackend.Controllers
{
    [ApiController]
    [Route("test")]
    public class tests : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> testCosmosConn(string? containerName, string? partitionkey, string json)
        {
            // For testing cosmos db connection and inserting a item into Inventio login. 

            Database database = await InventioAdminCosmosDB.client.CreateDatabaseIfNotExistsAsync(Settings.CosmosDbName);
// Partitionkey are made when making the container, e.g. userName for InventioLogin container partitionkey could also be consider primarykey and this is where it 
// should look.
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, "/" + partitionkey); 
            // Container container = await database.CreateContainerIfNotExistsAsync(containerName, "/" + partitionkey); 

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            using(var response = await container.UpsertItemStreamAsync(stream, new PartitionKey(jsonElement.GetProperty(partitionkey).GetString())))
            {
                if(!response.IsSuccessStatusCode)
                {
                    return BadRequest("failed");
                }
                string content = new StreamReader(response.Content, Encoding.UTF8).ReadToEnd();

                return Ok(content);
            }
        }
    }
}