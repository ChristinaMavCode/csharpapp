using CSharpApp.Core.Commands;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly CSharpAppClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IOptions<RestApiSettings> restApiSettings, ILogger<ProductsService> logger, CSharpAppClient httpClient)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        try
        {
            var content = await _httpClient.GetDataAsync(_restApiSettings.Products);
            var res = JsonSerializer.Deserialize<List<Product>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message} {ex.StackTrace}");

            return Array.Empty<Product>();
        }
    }

    public async Task<Product?> GetProduct(int productID)
    {
        try
        {
            var content = await _httpClient.GetDataWithAuthAsync($"{_restApiSettings.Products}/{productID}");
            var res = JsonSerializer.Deserialize<Product>(content);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message} {ex.StackTrace}");

            return null;
        }
    }

    public async Task<Product> CreateProduct(CreateProductCommand request)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(request, options);

            var content = await _httpClient.PostDataAsync(json, _restApiSettings.Products);
            var result = JsonSerializer.Deserialize<Product>(content, options);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message} {ex.StackTrace}");
            return null;
        }
    }
}