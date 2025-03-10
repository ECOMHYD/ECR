using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    // Get cart items for a specific user
    public async Task<List<CartDto>> GetUserCart(string userId)
    {
        int parsedUserId = int.Parse(userId);  // ✅ Convert userId to int

        return await _context.Carts
            .Include(c => c.Product)
            .Where(c => c.UserId == parsedUserId)  // ✅ Fix comparison
            .Select(c => new CartDto
            {
                ProductId = c.ProductId,
                ProductName = c.Product != null ? c.Product.Name : "Unknown",
                Quantity = c.Quantity,
                TotalPrice = c.TotalPrice
            })
            .ToListAsync();
    }

    // Add product to the cart
    public async Task AddToCart(string userId, int productId, int quantity)
    {
        int parsedUserId = int.Parse(userId);  // ✅ Convert userId to int

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new Exception("Product not found.");

        var existingCartItem = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == parsedUserId && c.ProductId == productId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price;
        }
        else
        {
            var cartItem = new Cart
            {
                UserId = parsedUserId,  // ✅ Use int UserId
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = quantity * product.Price
            };
            _context.Carts.Add(cartItem);
        }
        await _context.SaveChangesAsync();
    }

    // Update cart item quantity
    public async Task UpdateCartQuantity(string userId, int productId, int quantity)
    {
        int parsedUserId = int.Parse(userId);  // ✅ Convert userId to int

        var cartItem = await _context.Carts
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.UserId == parsedUserId && c.ProductId == productId);

        if (cartItem == null)
            throw new Exception("Item not found in cart.");

        if (cartItem.Product == null)
            throw new Exception("Product details not found.");

        cartItem.Quantity = quantity;
        cartItem.TotalPrice = quantity * cartItem.Product.Price;

        await _context.SaveChangesAsync();
    }

    // Remove an item from the cart
    public async Task RemoveFromCart(string userId, int productId)
    {
        int parsedUserId = int.Parse(userId);  // ✅ Convert userId to int

        var cartItem = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == parsedUserId && c.ProductId == productId);

        if (cartItem == null)
            throw new Exception("Item not found in cart.");

        _context.Carts.Remove(cartItem);
        await _context.SaveChangesAsync();
    }

    // Clear the user's entire cart
    public async Task ClearCart(string userId)
    {
        int parsedUserId = int.Parse(userId);  // ✅ Convert userId to int

        var cartItems = _context.Carts.Where(c => c.UserId == parsedUserId);
        _context.Carts.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    // Admin: Get all users and their cart details
    public async Task<List<UserCartDto>> GetAllUsersCarts()
    {
        var usersCarts = await _context.Users
            .Where(u => u.Role != "Admin")
            .Include(u => u.Carts)
            .ThenInclude(c => c.Product)
            .ToListAsync(); // Fetch all users first

        // 🛠️ Debugging: Print users and their cart count
        foreach (var user in usersCarts)
        {
            Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}, Cart Items: {user.Carts.Count}");
            foreach (var cart in user.Carts)
            {
                Console.WriteLine($"  - Product ID: {cart.ProductId}, Quantity: {cart.Quantity}, Total Price: {cart.TotalPrice}");
            }
        }

        // Convert users to DTOs
        var result = usersCarts.Select(u => new UserCartDto
        {
            UserId = u.Id.ToString(),
            UserName = u.Username,
            Email = u.Email,
            Carts = u.Carts.Select(c => new CartDto
            {
                ProductId = c.ProductId,
                ProductName = c.Product != null ? c.Product.Name : "Unknown",
                Quantity = c.Quantity,
                TotalPrice = c.TotalPrice
            }).ToList()
        }).ToList();

        return result;
    }
}
