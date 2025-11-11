using CSharpApp.Core.Commands;

namespace CSharpApp.Application.Products;

public class CategoriesService : ICategoriesService
{
    private readonly CSharpAppClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(IOptions<RestApiSettings> restApiSettings, ILogger<CategoriesService> logger, CSharpAppClient httpClient)
    {
        _restApiSettings = restApiSettings.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Category>> GetCategories()
    {
        try
        {
            var content = await _httpClient.GetDataWithAuthAsync(_restApiSettings.Categories);
            var res = JsonSerializer.Deserialize<List<Category>>(content);

            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message} {ex.StackTrace}");

            return Array.Empty<Category>();
        }
    }

    public async Task<Category?> GetCategory(int productID)
    {
        try
        {
            var content = await _httpClient.GetDataWithAuthAsync($"{_restApiSettings.Categories}/{productID}");
            var res = JsonSerializer.Deserialize<Category>(content);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message} {ex.StackTrace}");

            return null;
        }
    }

    public async Task<Category?> CreateCategory(CreateCategoryCommand request)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_restApiSettings.Categories))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string json = JsonSerializer.Serialize(request, options);

                var content = await _httpClient.PostDataWithAuthAsync(json, _restApiSettings.Categories);
                var result = JsonSerializer.Deserialize<Category>(content, options);
                return result;
            }
            else
            {
                _logger.LogError($"RestApiSettings.Categories is not provided");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message} {ex.StackTrace}");
            return null;
        }
    }
}