namespace GarmentFactoryAPI.Models
{
    public class AssemblyLine
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public int OrderDetailId { get; set; }
        public int TaskProductId { get; set; }
        public int UserId { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public TaskProduct TaskProduct { get; set; }
        public User User { get; set; }
    }
}
