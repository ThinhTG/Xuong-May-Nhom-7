using GarmentFactoryAPI.DTO;
using System.Collections.Generic;

namespace GarmentFactoryAPI.Interfaces
{
    public interface ICategoryService
    {
        ICollection<CategoryDTO> GetCategories();
        CategoryDTO GetCategoryById(int categoryId);
        bool CreateCategory(CategoryDTO categoryDto);
        bool UpdateCategory(CategoryDTO categoryDto);
        bool DeleteCategory(int categoryId);
    }
}
