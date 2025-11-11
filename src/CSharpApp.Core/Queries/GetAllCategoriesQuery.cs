using CSharpApp.Core.Interfaces;
using MediatR;

namespace CSharpApp.Core.Queries
{
    public record GetAllCategoriesQuery() : IRequest<IReadOnlyCollection<Category>>;

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyCollection<Category>>
    {
        private readonly ICategoriesService _categoriesService;

        public GetAllCategoriesQueryHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IReadOnlyCollection<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoriesService.GetCategories();
        }
    }
}
