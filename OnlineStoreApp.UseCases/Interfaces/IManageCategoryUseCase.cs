using OnlineStore.DTOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface IManageCategoryUseCase
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryAsync(int categoryId);
        Task<bool> AddCategoryAync(CategoryDTO categoryDTO);
        Task<bool> EditCategoryAsync(int categoryId, CategoryDTO categoryDTO);
        Task<bool> RemoveCategoryAsync(int categoryId);
    }
}
