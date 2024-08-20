using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Models;

namespace GarmentFactoryAPI.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        private new readonly DataContext _context;
        public RoleRepository(DataContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
