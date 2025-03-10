using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
    private string GetUserId()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        throw new UnauthorizedAccessException("User ID not found in token.");
    }
    return userId;
}


    // Get user's cart
    [Authorize(Roles = "User")]
   [HttpGet]
public async Task<IActionResult> GetUserCart()
{
    var userId = GetUserId(); // Automatically retrieves UserId
    var cart = await _cartService.GetUserCart(userId);
    return Ok(cart); // UserId is not returned
}
[Authorize(Roles = "User")]
[HttpPost("add/{productId}/{quantity}")]
public async Task<IActionResult> AddToCart(int productId, int quantity)
{
    var token = Request.Headers["Authorization"].ToString();
    Console.WriteLine($"Received Token: {token}");

    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
    {
        Console.WriteLine("❌ User ID not found in token.");
        return Unauthorized("User not authenticated");
    }

    await _cartService.AddToCart(userId, productId, quantity);
    return Ok("Product added to cart successfully.");
}


// Update quantity of a product in cart
    [Authorize(Roles = "User")]
    [HttpPut("{productId}/{quantity}")]
    public async Task<IActionResult> UpdateCartQuantity(int productId, int quantity)
    {
        var userId = GetUserId();
        await _cartService.UpdateCartQuantity(userId, productId, quantity);
        return Ok("Cart updated successfully.");
    }

    // Remove product from cart
    [Authorize(Roles = "User")]
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = GetUserId();
        await _cartService.RemoveFromCart(userId, productId);
        return Ok("Product removed from cart.");
    }

    // Clear entire cart
    [Authorize(Roles = "User")]
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();
        await _cartService.ClearCart(userId);
        return Ok("Cart cleared.");
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("admin/all-users-carts")]
    public async Task<IActionResult> GetAllUsersCarts()
     {
    var usersCarts = await _cartService.GetAllUsersCarts();
    return Ok(usersCarts);
}

}
