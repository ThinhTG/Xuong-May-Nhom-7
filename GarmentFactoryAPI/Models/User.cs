using System.Text.Json.Serialization;

namespace GarmentFactoryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public int RoleId { get; set; }

        public bool IsDeleted { get; set; }

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
