using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using MediatR;
using System;

namespace CSharpApp.Core.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<Product?>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductsService _productsService;

        public GetProductByIdQueryHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productsService.GetProduct(request.Id);
        }
    }
}
