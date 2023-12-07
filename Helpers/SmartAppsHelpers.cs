using InventioAdminBackend.Models;

namespace InventioAdminBackend.Helpers
{
    public class SmartAppsHelpers
    {
        public static async Task<Dictionary<string,int>> CountInstalledSmartApps(List<CustomerSMARTapps> customerSMARTapps)
        {
            Dictionary<string, int> smartAppsCounts = new()
            {
                {"SMARTean", 0},
                {"SMARTedit", 0},
                {"SMARTbank", 0},
                {"SMARTapi", 0},
                {"SMARTbilag", 0}
            };

            foreach (var customers in customerSMARTapps)
            {
                foreach (var smartapps in customers.SmartApps)
                {
                    if(smartAppsCounts.ContainsKey(smartapps))
                    {
                        smartAppsCounts[smartapps]++;
                    }
                }
            }
            
            return smartAppsCounts;
        }
    }
}