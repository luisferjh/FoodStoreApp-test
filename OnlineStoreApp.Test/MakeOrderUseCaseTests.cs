using Moq;
using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;
using OnlineStoreApp.UseCases.UseCases;

namespace OnlineStoreApp.Test
{
    public class MakeOrderUseCaseTests
    {
        private readonly Mock<IUnitOfWorkAdapter> _unitOfWorkAdapterMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ISenderEmailService> _senderEmailServiceMock;
        private readonly IMakeOrderUseCase _makeOrderUseCase;

        public MakeOrderUseCaseTests()
        {
            _unitOfWorkAdapterMock = new Mock<IUnitOfWorkAdapter>();
            _userServiceMock = new Mock<IUserService>();
            _senderEmailServiceMock = new Mock<ISenderEmailService>();
            _makeOrderUseCase = new MakeOrderUseCase(_unitOfWorkAdapterMock.Object, _userServiceMock.Object, _senderEmailServiceMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnOrderResponseDTO()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Date = DateTime.Now,
                Status = "Success",
                Total = 100,
                UserId = 1,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { Id = 1, FoodId = 1, Quantity = 2, SubTotal = 50, Food = new Food { Name = "Pizza" } }
                }
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.GetAsync(orderId)).ReturnsAsync(order);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _makeOrderUseCase.GetAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Orden.Id);
        }

        [Fact]
        public void Get_ShouldReturnOrderResponseDTO()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Date = DateTime.Now,
                Status = "Success",
                Total = 100,
                UserId = 1,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { Id = 1, FoodId = 1, Quantity = 2, SubTotal = 50, Food = new Food { Name = "Pizza" } }
                }
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.Get(orderId)).Returns(order);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = _makeOrderUseCase.Get(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Orden.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfOrderResponseDTO()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Status = "Success",
                    Total = 100,
                    UserId = 1,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail { Id = 1, FoodId = 1, Quantity = 2, SubTotal = 50, Food = new Food { Name = "Pizza" } }
                    }
                }
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.GetAllAsync()).ReturnsAsync(orders);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _makeOrderUseCase.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task PlaceOrderAsync_ShouldReturnSuccessResultService()
        {
            // Arrange            

            var orderRequest = new OrderRequestDTO
            {
                Order = new OrderDTO { Total = 100 },
                OrderDetails = new List<OrderDetailDTO>
                {
                    new OrderDetailDTO { FoodId = 1, Quantity = 2, SubTotal = 21.08m }
                }
            };

            var user = new User { Id = 1, Email = "test@example.com", Name = "Test User" };
            var food = new Food { Id = 1, Name = "Pizza", QuantityAvailable = 10 };
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = 1,
                Date = DateTime.Now,
                Total = 21.08m,
                Status = "Success"
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.UserRepository.Get(It.IsAny<string>())).ReturnsAsync(user);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.FoodRepository.GetAsync(It.IsAny<int>())).ReturnsAsync(food);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.GetAsync(It.IsAny<Guid>())).ReturnsAsync(order);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.InsertOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.InsertOrderDetailAsync(It.IsAny<OrderDetail>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.FoodRepository.UpdateStockFoodAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(true);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);
            _userServiceMock.Setup(u => u.GetEmailUserAuth()).Returns("test@example.com");

            // Act
            var result = await _makeOrderUseCase.PlaceOrderAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task PlaceOrderAsync_ShouldReturnFailureResultService_WhenNoStockAvailable()
        {
            // Arrange
            var orderRequest = new OrderRequestDTO
            {
                Order = new OrderDTO { Total = 100 },
                OrderDetails = new List<OrderDetailDTO>
                {
                    new OrderDetailDTO { FoodId = 1, Quantity = 2, SubTotal = 50 }
                }
            };

            var user = new User { Id = 1, Email = "test@example.com", Name = "Test User" };
            var food = new Food { Id = 1, Name = "Pizza", QuantityAvailable = 0 };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.UserRepository.Get(It.IsAny<string>())).ReturnsAsync(user);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.FoodRepository.GetAsync(It.IsAny<int>())).ReturnsAsync(food);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.OrderRepository.InsertOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);
            _userServiceMock.Setup(u => u.GetEmailUserAuth()).Returns("test@example.com");

            // Act
            var result = await _makeOrderUseCase.PlaceOrderAsync(orderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("There is no stock available about the food Pizza", result.ErrorMessage);
        }
    }
}