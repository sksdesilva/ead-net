using UserRegistrationAPI.Models;

namespace UserRegistrationAPI.Services
{
    public interface IProductService
    {
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(string id, Product product);
        Task DeleteProductAsync(string id);
        Task ActivateProductAsync(string id);
        Task DeactivateProductAsync(string id);

        Task<List<Product>> GetAllProductsAsync();

        Task<List<Product>> GetProductsByVendorAsync(string vendorName);
    }
}
