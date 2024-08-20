namespace GarmentFactoryAPI.DTO
{
    public class TaskProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        // Optionally include other properties if needed
        // e.g., public int UserId { get; set; }
        // e.g., public ICollection<int> AssemblyLineIds { get; set; }
    }
}
