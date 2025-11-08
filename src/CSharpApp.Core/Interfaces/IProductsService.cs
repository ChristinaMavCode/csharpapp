using CSharpApp.Core.Commands;

namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProducts();

    Task<Product?> GetProduct(int productID);

    Task<Product?> CreateProduct(CreateProductCommand request);
}