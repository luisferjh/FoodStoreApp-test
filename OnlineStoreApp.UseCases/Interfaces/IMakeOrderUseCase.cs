using OnlineStore.DTOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface IMakeOrderUseCase
    {
        Task<OrderResponseDTO> GetAsync(int orderId);
        OrderResponseDTO Get(int orderId);
        Task<List<OrderResponseDTO>> GetAllAsync();
        Task<ResultService> PlaceOrderAsync(OrderRequestDTO orderRequest);
    }
}
