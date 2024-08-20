using GarmentFactoryAPI.Models;
using System.Collections.Generic;

namespace GarmentFactoryAPI.Interfaces
{
    public interface ITaskProductRepository
    {
        ICollection<TaskProduct> GetTaskProducts();
        TaskProduct GetTaskProductById(int taskProductId);
        ICollection<TaskProduct> GetTaskProductsByName(string taskProductName);
        bool HasTaskProduct(int taskProductId);
        bool HasTaskProduct(string taskProductName);
        bool CreateTaskProduct(TaskProduct taskProduct);
        bool UpdateTaskProduct(TaskProduct taskProduct);
        bool DeleteTaskProduct(TaskProduct taskProduct);
        bool Save();
    }
}
