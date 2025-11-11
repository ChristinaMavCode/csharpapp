using CSharpApp.Core.Interfaces;
using MediatR;

namespace CSharpApp.Core.Commands
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string? Title { get; set; }

        public int Price { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }        
        public List<string> Images { get; set; } = new List<string>(); 
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product?>
    {
        private readonly IProductsService _productsService;

        public CreateProductCommandHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<Product?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await _productsService.CreateProduct(request);
        }
    }
}
