using OnlineStore.DTOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface IManageFoodUseCase
    {
        Task<List<FoodDTO>> GetAllFoodsInCatalogAsync();
        Task<FoodDTO> GetFoodInCatalogAsync(int idfood);
        Task<bool> AddFoodToCatalogAsync(CreateFoodDTO foodDTO);
        Task<bool> EditFoodInCatalogAsync(int foodId, CreateFoodDTO foodDTO);
        Task<bool> RemoveFoodFromCatalogAsync(int foodId);
        Task<List<FoodDTO>> GetAvailableFoodsAsync();

    }
}
