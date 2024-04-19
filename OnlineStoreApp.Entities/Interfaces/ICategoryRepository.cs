using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Entities.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetAsync(int idCategory);
        Task CreateCategoryAsync(Category category);
        void UpdateCategory(Category category);
        Task DeleteCategoryAsync(int IdCategory);
    }
}
