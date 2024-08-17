using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Repositories;

namespace GarmentFactoryAPI.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataContext _context;
        //private CategoryRepository? _categoryRepository;
        //private ProductRepository? _productRepository;
        //private OrderRepository? _orderRepository;
        //private OrderDetailRepository? _orderDetailRepository;
        //private AssemblyLineRepository? _assemblyLineRepository;
        //private TaskProductRepository? _taskProductRepository;
        private UserRepository? _userRepository;
        //private RoleRepository? _roleRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

/*        public CategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);
            }
        }

        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }

        public OrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new OrderRepository(_context);
            }
        }

        public OrderDetailRepository OrderDetailRepository
        {
            get
            {
                return _orderDetailRepository ??= new OrderDetailRepository(_context);
            }
        }

        public AssemblyLineRepository AssemblyLineRepository
        {
            get
            {
                return _assemblyLineRepository ??= new AssemblyLineRepository(_context);
            }
        }

        public TaskProductRepository TaskProductRepository
        {
            get
            {
                return _taskProductRepository ??= new TaskProductRepository(_context);
            }
        }*/

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

/*        public RoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ??= new RoleRepository(_context);
            }
        }*/

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
