using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProducts() =>
        await _context.Products.Include(p => p.Category).ToListAsync();

    public async Task<Product> GetProductById(int id) =>
        await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

    public async Task<List<Product>> GetProductsByCategory(int categoryId) =>
        await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();

  public async Task AddProduct(int categoryId, Product product)
{
    var category = await _context.Categories.FindAsync(categoryId);
    if (category == null)
    {
        throw new Exception("Category not found.");
    }

    product.CategoryId = categoryId;
    product.Category = null;

    _context.Products.Add(product);
    await _context.SaveChangesAsync();
}


   public async Task UpdateProduct(Product product)
{
    var existingProduct = await _context.Products.FindAsync(product.ProductId);
    if (existingProduct == null)
    {
        throw new Exception("Product not found.");
    }

    // Update product details
    existingProduct.Name = product.Name;
    existingProduct.Price = product.Price;
    existingProduct.Quantity = product.Quantity;

    _context.Products.Update(existingProduct);
    await _context.SaveChangesAsync();
}


    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
