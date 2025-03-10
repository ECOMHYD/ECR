using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();
    Task<Product> GetProductById(int id);
    Task<List<Product>> GetProductsByCategory(int categoryId);
    Task AddProduct(int categoryId, Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(int id);
}
