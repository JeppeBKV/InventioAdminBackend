using InventioAdminBackend.Models;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InventioAdminBackend.Helpers
{
    public class CosmosHelpers
    {
        public static async Task<(bool, string)> InsertIntoCosmos(string containerName, string partitionkey, string json)
        {
            Database database = await InventioAdminCosmosDB.client.CreateDatabaseIfNotExistsAsync(Settings.CosmosDbName);
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, "/" + partitionkey); 
            
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            using var response = await container.UpsertItemStreamAsync(stream, new PartitionKey(jsonElement.GetProperty(partitionkey).GetString()));
            
            string content = "";
            bool statusCode = true;
            
            if(!response.IsSuccessStatusCode)
            {
                statusCode = false;
                content = response.ErrorMessage; 
            }
            
            content = new StreamReader(response.Content, Encoding.UTF8).ReadToEnd();

            return (statusCode, content);
        }
    }
}