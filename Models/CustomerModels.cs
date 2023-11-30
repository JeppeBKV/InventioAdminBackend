namespace InventioAdminBackend.Models
{
    public class CustomerInfo
    {
        public string? id { get; set; }
        public string? TenantId { get; set; }
        public string? CvrNumber { get; set; }
        public string? CompanyName { get; set;}
        public string? Licenstype { get; set; }
        public string? Status { get; set; }
        public string? Country { get; set; }
        public string? Users { get; set; }
        public string? Created { get; set; }
    }
}