namespace InventioAdminBackend.Models
{
    public class SMARTapps
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Installed { get; set; }
    }
    public class CustomerSMARTapps
    {
        public string id { get; set; }
        public List<string> SmartApps { get; set; }
    }
}