using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Queries;
using FluentAssertions;
using Moq;

namespace Application.Tests.Handlers
{
    public class GetProductByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var mockService = new Mock<IProductsService>();

            var product = new Product
            {
                Id = 1,
                Title = "Laptop",
                Price = 1200,
                Description = "Gaming Laptop",
                Category = new Category() { Id = 1 }
            };

            mockService
                .Setup(s => s.GetProduct(1))
                .ReturnsAsync(product);

            var handler = new GetProductByIdQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Laptop");
        }

        [Fact]
        public async Task Handle_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var mockService = new Mock<IProductsService>();
            mockService.Setup(s => s.GetProduct(999)).ReturnsAsync((Product?)null);

            var handler = new GetProductByIdQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(999), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}