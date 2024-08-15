namespace GarmentFactoryAPI.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        
    }
}
