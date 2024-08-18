namespace GarmentFactoryAPI.DTO
{
    public class ProductDTO
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public double Price { get; set; }
            public int CategoryId { get; set; }
            public int UserId { get; set; }
    }
}
