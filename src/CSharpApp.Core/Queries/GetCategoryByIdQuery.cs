using CSharpApp.Core.Interfaces;
using MediatR;

namespace CSharpApp.Core.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Category?>;

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly ICategoriesService _categoriesService;

        public GetCategoryByIdQueryHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoriesService.GetCategory(request.Id);
        }
    }
}
