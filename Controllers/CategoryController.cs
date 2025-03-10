using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // Admin & User - View Categories
    [HttpGet]
    public async Task<IActionResult> GetCategories() => Ok(await _categoryRepository.GetAllCategories());

    // Admin - View Category by ID
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryRepository.GetCategoryById(id);
        return category == null ? NotFound() : Ok(category);
    }

    // Admin - Add Category
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] Category category)
    {
        await _categoryRepository.AddCategory(category);
        return Ok("Category added successfully.");
    }

    // Admin - Update Category
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
    {
        category.CategoryId = id;
        await _categoryRepository.UpdateCategory(category);
        return Ok("Category updated successfully.");
    }

    // Admin - Delete Category
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryRepository.DeleteCategory(id);
        return Ok("Category deleted successfully.");
    }
}
