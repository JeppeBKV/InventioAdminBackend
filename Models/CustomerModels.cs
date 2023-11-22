namespace InventioAdminBackend.Models
{
    public class CustomerInfo
    {
        public required string id { get; set; }
        public required string TenantId { get; set; }
        public required string cvrNummer { get; set; }
        public required string Virksomhedsnavn { get; set;}
        public required string Licenstype { get; set; }
        public required string Status { get; set; }
    }
}