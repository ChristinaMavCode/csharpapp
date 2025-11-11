using CSharpApp.Core.Interfaces;
using MediatR;

namespace CSharpApp.Core.Commands
{
    public class CreateCategoryCommand : IRequest<Category>
    {
        public string? Name { get; set; }
      
        public string? Image { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category?>
    {
        private readonly ICategoriesService _categoriesService;

        public CreateCategoryCommandHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<Category?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoriesService.CreateCategory(request);
        }
    }
}
