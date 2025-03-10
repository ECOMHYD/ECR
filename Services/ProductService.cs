using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllProducts() => await _productRepository.GetAllProducts();

    public async Task<Product> GetProductById(int id) => await _productRepository.GetProductById(id);

    public async Task<List<Product>> GetProductsByCategory(int categoryId) => await _productRepository.GetProductsByCategory(categoryId);

    public async Task AddProduct(int categoryId, Product product)
{
    await _productRepository.AddProduct(categoryId, product);
}


    public async Task UpdateProduct(Product product) => await _productRepository.UpdateProduct(product);

    public async Task DeleteProduct(int id) => await _productRepository.DeleteProduct(id);
}
