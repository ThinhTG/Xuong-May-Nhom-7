namespace GarmentFactoryAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail>  OrderDetails { get; set; }
    }
}
