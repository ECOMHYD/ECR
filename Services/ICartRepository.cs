using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICartRepository
{
    Task<List<CartDto>> GetUserCart(string userId);
    Task AddToCart(string userId, int productId, int quantity);
    Task UpdateCartQuantity(string userId, int productId, int quantity);
    Task RemoveFromCart(string userId, int productId);
    Task ClearCart(string userId);
    Task<List<UserCartDto>> GetAllUsersCarts();
}
