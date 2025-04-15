using OnlineStore.DTOs;

namespace OnlineStoreApp.UseCases.Interfaces
{
    public interface IMakeOrderUseCase
    {
        Task<OrderResponseDTO> GetAsync(Guid orderId);
        OrderResponseDTO Get(Guid orderId);
        Task<List<OrderResponseDTO>> GetAllAsync();
        Task<ResultService> PlaceOrderAsync(OrderRequestDTO orderRequest);
    }
}
