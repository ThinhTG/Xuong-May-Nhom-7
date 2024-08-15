namespace GarmentFactoryAPI.Models
{
    public class TaskProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<AssemblyLine> AssemblyLines { get; set; }
    }
}
