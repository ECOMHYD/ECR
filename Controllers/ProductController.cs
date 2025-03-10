using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
[Authorize]
[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // User & Admin - Get all products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    // User & Admin - Get product by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetProductById(id);
        return product == null ? NotFound() : Ok(product);
    }

    // User & Admin - Get products by category
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategory(categoryId);
        return Ok(products);
    }

    // Admin - Add new product
    // Admin - Add new product
[Authorize(Roles = "Admin")]
[HttpPost("category/{categoryId}")]
public async Task<IActionResult> AddProduct(int categoryId, [FromBody] Product product)
{
    await _productService.AddProduct(categoryId, product);
    return Ok("Product added successfully.");
}


    // Admin - Update product
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        product.ProductId = id;
        await _productService.UpdateProduct(product);
        return Ok("Product updated successfully.");
    }

    // Admin - Delete product
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProduct(id);
        return Ok("Product deleted successfully.");
    }
}
