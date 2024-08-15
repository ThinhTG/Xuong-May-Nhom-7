using Microsoft.EntityFrameworkCore;

namespace GarmentFactoryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
