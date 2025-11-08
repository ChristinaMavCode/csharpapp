using CSharpApp.Core.Commands;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.Tests.Handlers
{
    public class CreateProductCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnCreatedProduct()
        {
            // Arrange
            var mockService = new Mock<IProductsService>();
            var command = new CreateProductCommand
            {
                Title = "Test Product",
                Price = 100,
                Description = "Test Description",
                CategoryId = 36
            };

            var expectedProduct = new Product
            {
                Id = 1,
                Title = command.Title,
                Price = command.Price,
                Description = command.Description,
                Category = new Category() { Id = command.CategoryId }
            };

            mockService
                .Setup(s => s.CreateProduct(command))
                .ReturnsAsync(expectedProduct);

            var handler = new CreateProductCommandHandler(mockService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeNull();
            result.Title.Should().Be(command.Title);
        }
    }
}