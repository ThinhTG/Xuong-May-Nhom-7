using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GarmentFactoryAPI.Models; // Assuming User and GarmentFactoryContext are in this namespace
using GarmentFactoryAPI; // Assuming IUserRepository is in this namespace
using GarmentFactoryAPI.Repositories; // Assuming GenericRepository is in this namespace
using System.Collections.Generic;
using GarmentFactoryAPI.Data;

namespace GarmentFactoryAPI.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private new readonly DataContext _context;

        public UserRepository(DataContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserWithProducts(int id)
        {
            return await _context.Users.Include(u => u.Products).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || user.Password != password)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password && u.IsActive == true);
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null;
        }




    }
}
