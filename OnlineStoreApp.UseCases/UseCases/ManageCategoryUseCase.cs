using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.UseCases
{
    public class ManageCategoryUseCase : IManageCategoryUseCase
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public ManageCategoryUseCase(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(categoryId);

            if (category is null)
                return null;

            return new CategoryDTO
            {
                Name = category.Name
            };
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories.Count() <= 0)
                return new List<CategoryDTO>();

            return categories.Select(s => new CategoryDTO { Name = s.Name }).ToList();
        }

        public async Task<bool> AddCategoryAync(CategoryDTO categoryDTO)
        {
            await _unitOfWork.CategoryRepository.CreateCategoryAsync(new Category
            {
                Name = categoryDTO.Name,
            });
            return await _unitOfWork.SaveAsync();

        }

        public async Task<bool> EditCategoryAsync(int categoryId, CategoryDTO categoryDTO)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(categoryId);
            if (category is null)
            {
                return false;
            }
            category.Name = categoryDTO.Name;
            _unitOfWork.CategoryRepository.UpdateCategory(category);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveCategoryAsync(int categoryId)
        {
            await _unitOfWork.CategoryRepository.DeleteCategoryAsync(categoryId);
            return await _unitOfWork.SaveAsync();
        }


    }
}
