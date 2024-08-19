namespace GarmentFactoryAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }

        public Category Category { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
