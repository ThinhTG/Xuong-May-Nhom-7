using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Models;
using GermentFactoryAPI.Services;

namespace GarmentFactoryAPI.Services
{
    public interface IRoleService
    {
        Task<bool> RoleExists(int roleId);
    }
    public class RoleService : IRoleService
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleService(DataContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }

        // Existing methods...

        public async Task<bool> RoleExists(int roleId)
        {
            // Check if a role with the given roleId exists in the database
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            return role != null;
        }
    }
}
