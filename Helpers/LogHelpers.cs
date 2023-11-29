using System.Reflection;
using InventioAdminBackend.Models;
using Newtonsoft.Json;


namespace InventioAdminBackend.Helpers
{
    public class LogHelpers 
    {
        public static async Task LogCreateCustomer(string userId, string statusMessage)
        {
            dynamic jsonobject = JsonConvert.DeserializeObject<dynamic>(statusMessage);
            
            object logDetails = new
            {
                CustomerId = $"{jsonobject.id}",
                CustomerName = $"{jsonobject.Virksomhedsnavn}"
            };

            Log log = new() 
            {
                UserId = userId, 
                LogDetails = logDetails, 
                LogMessage = "Created new customer"
            };

            string json = System.Text.Json.JsonSerializer.Serialize(log);            
            await CosmosHelpers.InsertIntoCosmos("InventioLogs", "id", json);
        }
        
        public static async Task LogDeleteCustomer(string userId, string customerId)
        {
            object logDetails = new
            {
                CustomerId = $"{customerId}"
            };
            Log log = new() 
            {
                UserId = userId,
                LogDetails = logDetails,
                LogMessage = "Deleted customer"
            };
            string json = System.Text.Json.JsonSerializer.Serialize(log);
            await CosmosHelpers.InsertIntoCosmos("InventioLogs", "id", json);
        }

        public static async Task LogEditCustomer(string userId, CustomerInfo logdata)
        {

            Type objectType = logdata.GetType();
            PropertyInfo[] properties = objectType.GetProperties();

            Dictionary<string, object> nonNullValues = new();

            foreach(PropertyInfo property in properties)
            {
                object value = property.GetValue(logdata);
                if(value != null)
                {
                    nonNullValues.Add(property.Name, value);
                }
            }

            Log log = new()
            {
                UserId = userId,
                LogDetails = nonNullValues,
                LogMessage = "Edited customer"
            };
            string json = System.Text.Json.JsonSerializer.Serialize(log);
            await CosmosHelpers.InsertIntoCosmos("InventioLogs", "id", json);
            
        }
    }
} 
