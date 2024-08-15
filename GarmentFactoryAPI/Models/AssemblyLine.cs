namespace GarmentFactoryAPI.Models
{
    public class AssemblyLine
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public TaskProduct TaskProduct { get; set; }
    }
}
