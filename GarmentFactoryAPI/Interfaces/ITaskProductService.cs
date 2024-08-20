using GarmentFactoryAPI.DTO;
using System.Collections.Generic;

namespace GarmentFactoryAPI.Interfaces
{
    public interface ITaskProductService
    {
        ICollection<TaskProductDTO> GetTaskProducts();
        TaskProductDTO GetTaskProductById(int taskProductId);
        ICollection<TaskProductDTO> GetTaskProductsByName(string taskProductName);
        bool CreateTaskProduct(TaskProductDTO taskProductDto);
        bool UpdateTaskProduct(int taskProductId, TaskProductDTO taskProductDto);
        bool DeleteTaskProduct(int taskProductId);
    }
}
