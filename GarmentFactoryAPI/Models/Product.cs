namespace GarmentFactoryAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        public Category Category { get; set; }
    }
}
