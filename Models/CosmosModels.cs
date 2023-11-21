namespace InventioAdminBackend.Models
{
    public class InventioLoginModel
    {
        
        public string id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public InventioLoginModel(string id, string username, string password)
        {
            this.id = id;
            this.userName = username;
            this.password = password;
        }
    }
}