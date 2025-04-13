using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.UseCases
{
    public class ManageCategoryUseCase : IManageCategoryUseCase
    {
        private readonly IUnitOfWorkAdapter _unitOfWorkAdapter;

        public ManageCategoryUseCase(IUnitOfWorkAdapter unitOfWorkAdapter)
        {
            _unitOfWorkAdapter = unitOfWorkAdapter;
        }

        public async Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var category = await _unitOfWork.UnitOfWorkRepositories.CategoryRepository.GetAsync(categoryId);

            if (category is null)
                return null;

            return new CategoryDTO
            {
                Name = category.Name
            };
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var categories = await _unitOfWork.UnitOfWorkRepositories.CategoryRepository.GetAllAsync();

            if (categories.Count() <= 0)
                return new List<CategoryDTO>();

            return categories.Select(s => new CategoryDTO { Name = s.Name }).ToList();
        }

        public async Task<bool> AddCategoryAync(CategoryDTO categoryDTO)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            await _unitOfWork.UnitOfWorkRepositories.CategoryRepository.CreateCategoryAsync(new Category
            {
                Name = categoryDTO.Name,
            });
            return await _unitOfWork.SaveAsync();

        }

        public async Task<bool> EditCategoryAsync(int categoryId, CategoryDTO categoryDTO)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var category = await _unitOfWork.UnitOfWorkRepositories.CategoryRepository.GetAsync(categoryId);
            if (category is null)
            {
                return false;
            }
            category.Name = categoryDTO.Name;
            _unitOfWork.UnitOfWorkRepositories.CategoryRepository.UpdateCategory(category);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveCategoryAsync(int categoryId)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            await _unitOfWork.UnitOfWorkRepositories.CategoryRepository.DeleteCategoryAsync(categoryId);
            return await _unitOfWork.SaveAsync();
        }


    }
}
