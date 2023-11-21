namespace InventioAdminBackend.Models
{
    public class InventioLoginModel
    {
        
        public string id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public InventioLoginModel(string id, string username, string password)
        {
            this.id = id;
            this.UserName = username;
            this.Password = password;
        }
    }
}