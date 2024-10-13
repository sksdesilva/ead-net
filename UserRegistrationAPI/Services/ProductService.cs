using MongoDB.Driver;
using UserRegistrationAPI.Models;
using Microsoft.Extensions.Options;

namespace UserRegistrationAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IMongoClient client, IOptions<MongoDbSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<Product>("Products");
        }

        public async Task CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

       public async Task UpdateProductAsync(string id, Product updatedProduct)
{
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);  // Filter by the immutable _id field
    var update = Builders<Product>.Update
        .Set(p => p.Name, updatedProduct.Name)
        .Set(p => p.Category, updatedProduct.Category)
        .Set(p => p.Price, updatedProduct.Price)
        .Set(p => p.quntity, updatedProduct.quntity)
        .Set(p => p.VendorName, updatedProduct.VendorName)
        .Set(p => p.IsActive, updatedProduct.IsActive);

    await _products.UpdateOneAsync(filter, update);
} 

        public async Task DeleteProductAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }

        public async Task ActivateProductAsync(string id)
        {
            var update = Builders<Product>.Update.Set(p => p.IsActive, true);
            await _products.UpdateOneAsync(p => p.Id == id, update);
        }

        public async Task DeactivateProductAsync(string id)
        {
            var update = Builders<Product>.Update.Set(p => p.IsActive, false);
            await _products.UpdateOneAsync(p => p.Id == id, update);
        }

         public async Task<List<Product>> GetProductsByVendorAsync(string vendorName)
        {
            return await _products.Find(product => product.VendorName == vendorName).ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
{
    return await _products.Find(_ => true).ToListAsync(); // Retrieve all products from the collection
}


        
    }
}
