namespace GarmentFactoryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<AssemblyLine> AssemblyLines { get; set; }
        public ICollection<TaskProduct> TaskProducts { get; set; }
    }
}
