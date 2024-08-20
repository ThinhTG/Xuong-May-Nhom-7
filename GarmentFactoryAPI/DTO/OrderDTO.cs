using GarmentFactoryAPI.Models;

namespace GarmentFactoryAPI.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public int UserId { get; set; }
        //public List<OrderDetailDTO> OrderDetails { get; set; }
        public List<OrderDetailSummaryDTO> OrderDetails { get; set; }
    }

    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public double Price { get; set; }
        
    }
    public class OrderDetailSummaryDTO
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }

}
