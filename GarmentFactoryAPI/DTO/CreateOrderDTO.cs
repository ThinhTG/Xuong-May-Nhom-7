using GarmentFactoryAPI.DTO;

namespace GarmentFactoryAPI.DTO
{
    public class CreateOrderDTO
    {
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public int UserId { get; set; }
        public List<CreateOrderDetailDTO> OrderDetails { get; set; }
    }

    public class CreateOrderDetailDTO
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }

}
