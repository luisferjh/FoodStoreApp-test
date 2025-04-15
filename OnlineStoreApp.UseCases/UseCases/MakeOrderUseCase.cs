using OnlineStore.DTOs;
using OnlineStore.DTOs.Enums;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.UseCases
{
    public class MakeOrderUseCase : IMakeOrderUseCase
    {
        private readonly IUnitOfWorkAdapter _unitOfWorkAdapter;
        private readonly IUserService _userService;
        private readonly ISenderEmailService _senderEmail;

        public MakeOrderUseCase(
            IUnitOfWorkAdapter unitOfWorkAdapter,
            IUserService userService,
            ISenderEmailService senderEmail)
        {
            _unitOfWorkAdapter = unitOfWorkAdapter;
            _userService = userService;
            _senderEmail = senderEmail;
        }

        public async Task<OrderResponseDTO> GetAsync(Guid orderId)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var order = await _unitOfWork.UnitOfWorkRepositories.OrderRepository.GetAsync(orderId);

            return new OrderResponseDTO
            {
                Orden = new OrderDto
                {
                    Id = order.Id,
                    Date = order.Date,
                    Status = order.Status,
                    Total = order.Total,
                    UserId = order.UserId
                },
                orderDetailDtos = order.OrderDetails.Select(w => new OrderDetailDto
                {
                    Id = w.Id,
                    Food = w.Food.Name,
                    FoodId = w.FoodId,
                    Quantity = w.Quantity,
                    SubTotal = w.SubTotal
                }).ToList()
            };
        }

        public OrderResponseDTO Get(Guid orderId)
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var order = _unitOfWork.UnitOfWorkRepositories.OrderRepository.Get(orderId);

            return new OrderResponseDTO
            {
                Orden = new OrderDto
                {
                    Id = order.Id,
                    Date = order.Date,
                    Status = order.Status,
                    Total = order.Total,
                    UserId = order.UserId
                },
                orderDetailDtos = order.OrderDetails.Select(w => new OrderDetailDto
                {
                    Id = w.Id,
                    Food = w.Food.Name,
                    FoodId = w.FoodId,
                    Quantity = w.Quantity,
                    SubTotal = w.SubTotal
                }).ToList()
            };
        }

        public async Task<List<OrderResponseDTO>> GetAllAsync()
        {
            var _unitOfWork = _unitOfWorkAdapter.Create();
            var orders = await _unitOfWork.UnitOfWorkRepositories.OrderRepository.GetAllAsync();

            return orders.Select(s => new OrderResponseDTO
            {
                Orden = new OrderDto
                {
                    Id = s.Id,
                    Date = s.Date,
                    Status = s.Status,
                    Total = s.Total,
                    UserId = s.UserId
                },
                orderDetailDtos = s.OrderDetails.Select(w => new OrderDetailDto
                {
                    Id = w.Id,
                    Food = w.Food.Name,
                    FoodId = w.FoodId,
                    Quantity = w.Quantity,
                    SubTotal = w.SubTotal
                }).ToList()
            }).ToList();
        }

        public async Task<ResultService> PlaceOrderAsync(OrderRequestDTO orderRequest)
        {
            try
            {
                var _unitOfWork = _unitOfWorkAdapter.Create();
                string email = _userService.GetEmailUserAuth();
                User user = await _unitOfWork.UnitOfWorkRepositories.UserRepository.Get(email);
                Order order = new Order();
                order.Status = PaymentState.Success.ToString();
                order.Total = orderRequest.Order.Total;
                order.UserId = user.Id;
                order.Date = DateTime.Now;
                order.Id = Guid.NewGuid();

                await _unitOfWork.UnitOfWorkRepositories.OrderRepository.InsertOrderAsync(order);
                //await _unitOfWork.SaveAsync();
                foreach (OrderDetailDTO orderDetailDto in orderRequest.OrderDetails)
                {
                    var food = await _unitOfWork.UnitOfWorkRepositories.FoodRepository.GetAsync(orderDetailDto.FoodId);
                    if (food.QuantityAvailable <= 0 ||
                        orderDetailDto.Quantity > food.QuantityAvailable)
                    {
                        return new ResultService
                        {
                            IsSuccess = false,
                            ErrorMessage = $"There is no stock available about the food {food.Name}",
                        };
                    }

                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        FoodId = orderDetailDto.FoodId,
                        Quantity = orderDetailDto.Quantity,
                        SubTotal = orderDetailDto.SubTotal,
                    };


                    await _unitOfWork.UnitOfWorkRepositories.OrderRepository.InsertOrderDetailAsync(orderDetail);
                    await _unitOfWork.UnitOfWorkRepositories.FoodRepository.UpdateStockFoodAsync(orderDetail.FoodId, orderDetail.Quantity);
                }

                var result = await _unitOfWork.SaveAsync();

                if (result)
                {
                    var orderDetails = await GetAsync(order.Id);
                    Thread thread = new Thread(() =>
                    {
                        _senderEmail.SendEmail(user.Email, user.Name, orderDetails);
                    });
                    thread.Start();

                    return new ResultService
                    {
                        IsSuccess = result,
                        Message = "Purchase successfully done!"
                    };
                }
                else
                {
                    return new ResultService
                    {
                        IsSuccess = false,
                        Message = "Purchase failed!"
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResultService
                {
                    IsSuccess = false
                };
            }

        }



    }
}
