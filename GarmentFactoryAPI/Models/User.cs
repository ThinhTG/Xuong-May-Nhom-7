using System.Text.Json.Serialization;

namespace GarmentFactoryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }

        [JsonIgnore]
        public ICollection<AssemblyLine> AssemblyLines { get; set; }

        [JsonIgnore]
        public ICollection<TaskProduct> TaskProducts { get; set; }
    }
}
