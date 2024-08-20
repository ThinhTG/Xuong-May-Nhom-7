using GarmentFactoryAPI.Models;

namespace GarmentFactoryAPI.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategoryById(int categoryId);
        ICollection<Category> GetCategoriesByName(string categoryName);

        bool HasCategory(int categoryId);
        bool HasCategory(string categoryName);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
