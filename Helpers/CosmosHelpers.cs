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
        // This is for insert/upserting in cosmos. This is made dynamic, so it can put items into any container in the inventio database
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
        // this is for validating the password when trying to login.
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

        public static async Task<(List<CustomerInfo>, string)> RetrieveCustomerItemsAsync()
        {
            Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
            Container container = database.GetContainer("InventioKunder");
            List<CustomerInfo> customers = new();
            try
            {   // Make a query static. hence its needs a model. 
                // QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c").WithParameter("@category", "yourcategoryvalue");
                QueryDefinition queryDefinition = new ("SELECT * FROM c");
                FeedIterator<CustomerInfo> queryResultSEtIterator = container.GetItemQueryIterator<CustomerInfo>(queryDefinition);

                while(queryResultSEtIterator.HasMoreResults)
                {
                    // reality should return this. 
                    FeedResponse<CustomerInfo> currentResultSet = await queryResultSEtIterator.ReadNextAsync();
                    foreach(CustomerInfo item in currentResultSet)
                    {
                        customers.Add(item);
                    }
                    
                }

            }
            catch(Exception ex)
            {
                return (customers, ex.Message);
            }
            return (customers, "No Errors");
        }
    }
}