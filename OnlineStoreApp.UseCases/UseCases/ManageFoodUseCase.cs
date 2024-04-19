using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.UseCases
{
    public class ManageFoodUseCase : IManageFoodUseCase
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public ManageFoodUseCase(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FoodDTO>> GetAllFoodsInCatalogAsync()
        {
            var foods = await _unitOfWork.FoodRepository.GetAllAsync();

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
            var food = await _unitOfWork.FoodRepository.GetAsync(idfood);

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
            await _unitOfWork.FoodRepository.CreateFoodAsync(new Food
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
            var food = await _unitOfWork.FoodRepository.GetAsync(foodId);

            if (food is null)
                return false;

            food.Name = foodDTO.Name;
            food.CategoryId = foodDTO.CategoryId;
            food.Description = foodDTO.Description;
            food.Price = foodDTO.Price;
            food.QuantityAvailable = foodDTO.QuantityAvailable;

            _unitOfWork.FoodRepository.UpdateFood(food);

            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveFoodFromCatalogAsync(int foodId)
        {
            await _unitOfWork.FoodRepository.DeleteFoodAsync(foodId);
            return await _unitOfWork.SaveAsync();
        }
    }
}
