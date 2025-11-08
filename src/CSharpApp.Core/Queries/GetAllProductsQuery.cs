using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using MediatR;
using System;

namespace CSharpApp.Core.Queries
{
    public record GetAllProductsQuery() : IRequest<IReadOnlyCollection<Product>>;

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<Product>>
    {
        private readonly IProductsService _productsService;

        public GetAllProductsQueryHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<IReadOnlyCollection<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productsService.GetProducts();
        }
    }
}
