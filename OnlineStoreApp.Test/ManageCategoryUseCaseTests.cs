using Moq;
using OnlineStore.DTOs;
using OnlineStoreApp.Entities.Interfaces;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.UseCases.Interfaces;
using OnlineStoreApp.UseCases.UseCases;

namespace OnlineStoreApp.Test
{
    public class ManageCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWorkAdapter> _unitOfWorkAdapterMock;
        private readonly IManageCategoryUseCase _manageCategoryUseCase;

        public ManageCategoryUseCaseTests()
        {
            _unitOfWorkAdapterMock = new Mock<IUnitOfWorkAdapter>();
            _manageCategoryUseCase = new ManageCategoryUseCase(_unitOfWorkAdapterMock.Object);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnCategoryDTO()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Electronics" };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.GetAsync(categoryId)).ReturnsAsync(category);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.GetCategoryAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Electronics", result.Name);
        }

        [Fact]
        public async Task GetCategoryAsync_ShouldReturnNull_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = 1;

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.GetAsync(categoryId)).ReturnsAsync((Category)null);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.GetCategoryAsync(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnListOfCategoryDTO()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Electronics", result[0].Name);
            Assert.Equal("Books", result[1].Name);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnEmptyList_WhenNoCategoriesFound()
        {
            // Arrange
            var categories = new List<Category>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldReturnTrue_WhenCategoryIsAdded()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Name = "Electronics" };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.CreateCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(true);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.AddCategoryAync(categoryDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditCategoryAsync_ShouldReturnTrue_WhenCategoryIsUpdated()
        {
            // Arrange
            var categoryId = 1;
            var categoryDTO = new CategoryDTO { Name = "Electronics" };
            var category = new Category { Id = categoryId, Name = "Books" };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.GetAsync(categoryId)).ReturnsAsync(category);
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.UpdateCategory(It.IsAny<Category>()));
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(true);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.EditCategoryAsync(categoryId, categoryDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditCategoryAsync_ShouldReturnFalse_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = 1;
            var categoryDTO = new CategoryDTO { Name = "Electronics" };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.GetAsync(categoryId)).ReturnsAsync((Category)null);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.EditCategoryAsync(categoryId, categoryDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveCategoryAsync_ShouldReturnTrue_WhenCategoryIsRemoved()
        {
            // Arrange
            var categoryId = 1;

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.UnitOfWorkRepositories.CategoryRepository.DeleteCategoryAsync(categoryId)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).ReturnsAsync(true);
            _unitOfWorkAdapterMock.Setup(u => u.Create()).Returns(unitOfWorkMock.Object);

            // Act
            var result = await _manageCategoryUseCase.RemoveCategoryAsync(categoryId);

            // Assert
            Assert.True(result);
        }
    }
}
