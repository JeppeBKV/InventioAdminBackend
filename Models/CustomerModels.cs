namespace InventioAdminBackend.Models
{
    public class CustomerInfo
    {
        public string? id { get; set; }
        public string? TenantId { get; set; }
        public string? cvrNummer { get; set; }
        public string? Virksomhedsnavn { get; set;}
        public string? Licenstype { get; set; }
        public string? Status { get; set; }
    }
}