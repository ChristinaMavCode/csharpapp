using CSharpApp.Core.Commands;

namespace CSharpApp.Core.Interfaces;

public interface ICategoriesService
{
    Task<IReadOnlyCollection<Category>> GetCategories();

    Task<Category?> GetCategory(int productID);

    Task<Category?> CreateCategory(CreateCategoryCommand request);
}