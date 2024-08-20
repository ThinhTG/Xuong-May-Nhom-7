using System.Diagnostics.CodeAnalysis;

namespace GarmentFactoryAPI.Models
{
    public class TaskProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [AllowNull]
        public User User { get; set; }
        public ICollection<AssemblyLine> AssemblyLines { get; set; }
    }
}
