using OnlineStore.DTOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface IMakeOrderUseCase
    {
        Task<List<OrderResponseDTO>> GetAllAsync();
        Task<ResultService> PlaceOrderAsync(OrderRequestDTO orderRequest);
    }
}
