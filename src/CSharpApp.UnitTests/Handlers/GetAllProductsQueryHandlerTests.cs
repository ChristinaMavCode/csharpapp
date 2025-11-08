using CSharpApp.Core.Commands;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Queries;
using FluentAssertions;
using Moq;

namespace Application.Tests.Handlers
{
    public class GetAllProductsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllProducts()
        {
            // Arrange
            var mockService = new Mock<IProductsService>();

            var products = new List<Product>
            {
                new Product { Id = 1, Title = "Product 1", Price = 50, Category = new Category(){ Id = 1 } },
                new Product { Id = 2, Title = "Product 2", Price = 100, Category = new Category(){ Id = 2 } },
            };

            mockService
                .Setup(s => s.GetProducts())
                .ReturnsAsync(products);

            var handler = new GetAllProductsQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Title.Should().Be("Product 1");
            result.Last().Price.Should().Be(100);
        }

        [Fact]
        public async Task Handle_WhenNoProducts_ShouldReturnEmptyList()
        {
            // Arrange
            var mockService = new Mock<IProductsService>();
            mockService.Setup(s => s.GetProducts()).ReturnsAsync(new List<Product>());

            var handler = new GetAllProductsQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}