namespace InventioAdminBackend.Models
{
    public class Log
    {
        public Guid id { get; set; }
        public string LogMessage { get; set; }
        public string? UserId { get; set; }
        public object? LogDetails { get; set; }
        public DateTime TimeStamp { get; set; }
        public Log()
        {
            id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
        }
    }
}