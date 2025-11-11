using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Queries;
using FluentAssertions;
using Moq;

namespace Application.Tests.Handlers
{
    public class GetCategoryByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCategoryExists_ShouldReturnProduct()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();

            var category = new Category
            {
                Id = 1,
                Name = "Book"
            };

            mockService.Setup(s => s.GetCategory(1)).ReturnsAsync(category);

            var handler = new GetCategoryByIdQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetCategoryByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Book");
        }

        [Fact]
        public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            mockService.Setup(s => s.GetCategory(999)).ReturnsAsync((Category?)null);

            var handler = new GetCategoryByIdQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetCategoryByIdQuery(999), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}