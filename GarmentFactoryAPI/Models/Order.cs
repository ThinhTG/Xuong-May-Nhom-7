namespace GarmentFactoryAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string CreatedBy { get; set; }
        public User User { get; set; }
    }
}
