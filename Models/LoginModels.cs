namespace InventioAdminBackend.Models
{
    public class UsernamePasswordModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class LoggedInId
    {
        public string Id { get; set; }
    }
}