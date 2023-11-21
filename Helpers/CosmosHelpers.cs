using InventioAdminBackend.Models;
using Microsoft.Azure.Cosmos;
using System.Linq.Expressions;
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

        public static async Task<string> RetrieveUserItemAsync(string username)
        {
            try
            {
                Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
                Container container = database.GetContainer("InventioLoginV2");

                string sqlQuery = "SELECT TOP 1 * FROM c WHERE c.UserName = @username";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery).WithParameter("@username", username);
                
                FeedIterator<InventioLoginModel> queryResult = container.GetItemQueryIterator<InventioLoginModel>(queryDefinition);
                while(queryResult.HasMoreResults)
                {
                    FeedResponse<InventioLoginModel> feedResponse = await queryResult.ReadNextAsync();
                    foreach(var user in feedResponse)
                    {
                        return user.Password;    
                    }
                }
                return "no users found";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

        }

        public static async Task<string> RetrieveItemsAsync(string containerName, string query)
        {
            Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
            Container container = database.GetContainer(containerName);

            try
            {
                QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("@category", "yourcategoryvalue");
                FeedIterator<string> queryResultSEtIterator = container.GetItemQueryIterator<string>(queryDefinition);

                while(queryResultSEtIterator.HasMoreResults)
                {
                    // reality should return this. 
                    FeedResponse<string> currentResultSet = await queryResultSEtIterator.ReadNextAsync();
                    foreach(string item in currentResultSet)
                    {
                        // do something. 
                        Console.Write("retrieved item: {item}");
                    }
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
            return "Ok";
        }
    }
}