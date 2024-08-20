using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace GarmentFactoryAPI.Services
{
    public class TaskProductService : ITaskProductService
    {
        private readonly ITaskProductRepository _taskProductRepository;

        public TaskProductService(ITaskProductRepository taskProductRepository)
        {
            _taskProductRepository = taskProductRepository;
        }

        public ICollection<TaskProductDTO> GetTaskProducts()
        {
            var taskProducts = _taskProductRepository.GetTaskProducts();
            return taskProducts.Select(tp => new TaskProductDTO
            {
                Id = tp.Id,
                Name = tp.Name
            }).ToList();
        }

        public TaskProductDTO GetTaskProductById(int id)
        {
            var taskProduct = _taskProductRepository.GetTaskProductById(id);
            if (taskProduct == null)
            {
                return null;
            }

            return new TaskProductDTO
            {
                Id = taskProduct.Id,
                Name = taskProduct.Name
            };
        }

        public ICollection<TaskProductDTO> GetTaskProductsByName(string taskProductName)
        {
            var taskProducts = _taskProductRepository.GetTaskProductsByName(taskProductName);
            return taskProducts.Select(tp => new TaskProductDTO
            {
                Id = tp.Id,
                Name = tp.Name
            }).ToList();
        }

        public bool CreateTaskProduct(TaskProductDTO taskProductDto)
        {
            var taskProduct = new TaskProduct
            {
                Name = taskProductDto.Name
            };

            return _taskProductRepository.CreateTaskProduct(taskProduct);
        }

        public bool UpdateTaskProduct(TaskProductDTO taskProductDto)
        {
            var taskProduct = new TaskProduct
            {
                Id = taskProductDto.Id,
                Name = taskProductDto.Name
            };

            return _taskProductRepository.UpdateTaskProduct(taskProduct);
        }

        public bool DeleteTaskProduct(int id)
        {
            var taskProduct = _taskProductRepository.GetTaskProductById(id);
            if (taskProduct == null)
            {
                return false;
            }

            taskProduct.IsActive = false;
            return _taskProductRepository.UpdateTaskProduct(taskProduct);
        }

        public ICollection<TaskProductDTO> GetAllTaskProductsFromData()
        {
            var taskProducts = _taskProductRepository.GetTaskProducts();
            return taskProducts.Select(tp => new TaskProductDTO
            {
                Id = tp.Id,
                Name = tp.Name
            }).ToList();
        }

        public bool UpdateTaskProduct(int taskProductId, TaskProductDTO taskProductDto)
        {
            throw new NotImplementedException();
        }
    }
}
