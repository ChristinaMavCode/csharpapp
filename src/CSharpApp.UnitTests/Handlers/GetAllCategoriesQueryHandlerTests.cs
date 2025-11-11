using CSharpApp.Core.Commands;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Queries;
using FluentAssertions;
using Moq;

namespace Application.Tests.Handlers
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllCategories()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();

            var products = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" },
            };

            mockService
                .Setup(s => s.GetCategories())
                .ReturnsAsync(products);

            var handler = new GetAllCategoriesQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Category 1");
        }

        [Fact]
        public async Task Handle_WhenNoCategories_ShouldReturnEmptyList()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            mockService.Setup(s => s.GetCategories()).ReturnsAsync(new List<Category>());

            var handler = new GetAllCategoriesQueryHandler(mockService.Object);

            // Act
            var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}