using CSharpApp.Core.Commands;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Application.Tests.Handlers
{
    public class CreateCategoryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnCreatedCategory()
        {
            // Arrange
            var mockService = new Mock<ICategoriesService>();
            var command = new CreateCategoryCommand
            {
                Name = "Books",
                Image = "https://api.lorem.space/image/book?w=150&h=220"
            };

            var expectedCategory = new Category
            {
                Id = 1,
                Name = command.Name,
                Image = command.Image,
                CreationAt = DateTime.Now,
                UpdatedAt = null
            };

            mockService
                .Setup(s => s.CreateCategory(command))
                .ReturnsAsync(expectedCategory);

            var handler = new CreateCategoryCommandHandler(mockService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
        }
    }
}