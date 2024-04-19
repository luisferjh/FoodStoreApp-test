using OnlineStore.DTOs;
using OnlineStore.DTOs.Enums;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.UseCases.UseCases
{
    public class MakeOrderUseCase : IMakeOrderUseCase
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IUserService _userService;
        private readonly ISenderEmailService _senderEmail;

        public MakeOrderUseCase(
            IUnitOfWorkRepository unitOfWork,
            IUserService userService,
            ISenderEmailService senderEmail)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _senderEmail = senderEmail;
        }

        public async Task<List<OrderResponseDTO>> GetAllAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();

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
                string email = _userService.GetEmailUserAuth();
                User user = await _unitOfWork.UserRepository.Get(email);
                Order order = new Order();
                order.Status = PaymentState.Success.ToString();
                order.Total = orderRequest.Order.Total;
                order.UserId = user.Id;
                order.Date = DateTime.Now;

                await _unitOfWork.OrderRepository.InsertOrderAsync(order);
                await _unitOfWork.SaveAsync();
                foreach (OrderDetailDTO orderDetailDto in orderRequest.OrderDetails)
                {
                    var food = await _unitOfWork.FoodRepository.GetAsync(orderDetailDto.FoodId);
                    if (food.QuantityAvailable <= 0 ||
                        orderDetailDto.Quantity >= food.QuantityAvailable)
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


                    await _unitOfWork.OrderRepository.InsertOrderDetailAsync(orderDetail);
                    await _unitOfWork.FoodRepository.UpdateStockFoodAsync(orderDetail.FoodId, orderDetail.Quantity);
                }

                var result = await _unitOfWork.SaveAsync();

                if (result)
                {
                    Thread thread = new Thread(() =>
                    {
                        _senderEmail.SendEmail(user.Email, user.Name);
                    });
                    thread.Start();
                    //Task.Run(() => _senderEmail.SendEmail(user.Email, user.Name)).Start();

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
