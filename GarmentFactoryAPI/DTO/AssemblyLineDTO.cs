namespace GarmentFactoryAPI.DTO
{
    public class AssemblyLineDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrderDetailId { get; set; }
        public int TaskProductId { get; set; }
        public int UserId { get; set; }
    }
}
