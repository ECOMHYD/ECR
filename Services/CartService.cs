using System.Collections.Generic;
using System.Threading.Tasks;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<List<CartDto>> GetUserCart(string userId) => await _cartRepository.GetUserCart(userId);
    public async Task AddToCart(string userId, int productId, int quantity) => await _cartRepository.AddToCart(userId, productId, quantity);
    public async Task UpdateCartQuantity(string userId, int productId, int quantity) => await _cartRepository.UpdateCartQuantity(userId, productId, quantity);
    public async Task RemoveFromCart(string userId, int productId) => await _cartRepository.RemoveFromCart(userId, productId);
    public async Task ClearCart(string userId) => await _cartRepository.ClearCart(userId);
    public async Task<List<UserCartDto>> GetAllUsersCarts() => await _cartRepository.GetAllUsersCarts();
}
