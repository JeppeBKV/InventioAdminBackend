using InventioAdminBackend.Models;
using Microsoft.Azure.Cosmos;
// using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public static async Task<(string, string?)> RetrieveUserItemAsync(string username)
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
                        return (user.Password, user.id);    
                    }
                }
                return ("no users found", null);
            }
            catch(Exception ex)
            {
                return (ex.Message, null);
            }

        }

        public static async Task<(List<CustomerInfo>, string)> RetrieveCustomerItemsAsync()
        {
            Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
            Container container = database.GetContainer("InventioKunderV2");
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

        public static async Task<(List<SMARTapps>, string)> RetrieveSmartAppsAsync()
        {
            Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
            Container container = database.GetContainer("InventioSMARTApps");
            List<SMARTapps> SmartApps = new();
            try
            {
                QueryDefinition queryDefinition = new ("SELECT * FROM c");
                FeedIterator<SMARTapps> queryResultSetIterator = container.GetItemQueryIterator<SMARTapps>(queryDefinition);
                while(queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<SMARTapps> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach(SMARTapps item in currentResultSet)
                    {
                        SmartApps.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                return (SmartApps, ex.Message);
            }
            return (SmartApps, "No Errors");
        }

        public static async Task<string> EditCustomer(CustomerInfo newCustomerInfo)
        {
            Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
            Container container = database.GetContainer("InventioKunderV2");
            try
            {
                ItemResponse<CustomerInfo> response = await container.ReadItemAsync<CustomerInfo>(newCustomerInfo.id, new PartitionKey(newCustomerInfo.id));
                CustomerInfo customer = response.Resource;
            
                Type customerType = typeof(CustomerInfo);
                foreach(PropertyInfo item in customerType.GetProperties())
                {
                    object newValue = item.GetValue(newCustomerInfo);

                    if(newValue != null)
                    {
                        item.SetValue(customer, newValue);
                    }
                }

                ItemResponse<CustomerInfo> updateResponse = await container.ReplaceItemAsync(customer, customer.id, new PartitionKey(newCustomerInfo.id));
                return "Item Updated successfully";
            }
            catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "Item not found";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> DeleteCustomerAsync(string customerId)
        {
            Database database = InventioAdminCosmosDB.client.GetDatabase(Settings.CosmosDbName);
            Container container = database.GetContainer("InventioKunderV2");
            try
            {
                ItemResponse<CustomerInfo> response = await container.DeleteItemAsync<CustomerInfo>(customerId, new Microsoft.Azure.Cosmos.PartitionKey(customerId));
                return "Deleted successfully";
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return "Customer not found";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

        }
        
    }
}