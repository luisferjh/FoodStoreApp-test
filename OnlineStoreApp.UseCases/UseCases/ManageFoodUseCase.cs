using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.UseCases
{
    public class ManageFoodUseCase : IManageFoodUseCase
    {
        private readonly IUnitOfWorkAdapter _unitOfWorkAdapter;

        public ManageFoodUseCase(IUnitOfWorkAdapter unitOfWorkAdapter)
        {
            _unitOfWorkAdapter = unitOfWorkAdapter;
        }

        public async Task<List<FoodDTO>> GetAllFoodsInCatalogAsync()
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var foods = await _unitOfWork.UnitOfWorkRepositories.FoodRepository.GetAllAsync();

            if (foods.Count() <= 0)
                return new List<FoodDTO>();

            return foods.Select(s => new FoodDTO
            {
                Category = s.Category.Name,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                QuantityAvailable = s.QuantityAvailable
            }).ToList();
        }

        public async Task<FoodDTO> GetFoodInCatalogAsync(int idfood)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var food = await _unitOfWork.UnitOfWorkRepositories.FoodRepository.GetAsync(idfood);

            if (food is null)
                return null;

            return new FoodDTO()
            {
                Category = food.Category.Name,
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                QuantityAvailable = food.QuantityAvailable
            };
        }

        public async Task<List<FoodDTO>> GetAvailableFoodsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddFoodToCatalogAsync(CreateFoodDTO foodDTO)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            await _unitOfWork.UnitOfWorkRepositories.FoodRepository.CreateFoodAsync(new Food
            {
                Name = foodDTO.Name,
                CategoryId = foodDTO.CategoryId,
                Description = foodDTO.Description,
                Price = foodDTO.Price,
                QuantityAvailable = foodDTO.QuantityAvailable
            });

            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> EditFoodInCatalogAsync(int foodId, CreateFoodDTO foodDTO)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var food = await _unitOfWork.UnitOfWorkRepositories.FoodRepository.GetAsync(foodId);

            if (food is null)
                return false;

            food.Name = foodDTO.Name;
            food.CategoryId = foodDTO.CategoryId;
            food.Description = foodDTO.Description;
            food.Price = foodDTO.Price;
            food.QuantityAvailable = foodDTO.QuantityAvailable;

            _unitOfWork.UnitOfWorkRepositories.FoodRepository.UpdateFood(food);

            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveFoodFromCatalogAsync(int foodId)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            await _unitOfWork.UnitOfWorkRepositories.FoodRepository.DeleteFoodAsync(foodId);
            return await _unitOfWork.SaveAsync();
        }
    }
}
