using Api.Models;
using Api.Storage;
using Microsoft.EntityFrameworkCore;

namespace Api.Service
{
    public class ShoppingCartService(AppDbContext dbContext)
    {
        public async Task CreateNewCartAsync(
            string userId, int productId, int quantity)
            {
                var cart = new ShoppingCart
                {
                    UserId = userId
                };

                await dbContext.ShoppingCarts.AddAsync(cart);
                await dbContext.SaveChangesAsync();

                var item = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = cart.Id,
                    Product = null
                };

                await dbContext.CartItems.AddAsync(item);
                await dbContext.SaveChangesAsync();
            }
        
        public async Task UpdateExistingCartAsync(
            ShoppingCart shoppingCart, int productId, int quantity
        )
        {
            var existingItem = shoppingCart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem is null && quantity > 0)
            {
                await dbContext.CartItems.AddAsync(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = shoppingCart.Id
                });
            }
            
            if (existingItem is not null && existingItem.Quantity + quantity <= 0)
            {
                dbContext.CartItems.Remove(existingItem);

                if (shoppingCart.CartItems.Count == 0)
                    dbContext.ShoppingCarts.Remove(shoppingCart);    
            }

            if (existingItem is not null && existingItem.Quantity + quantity > 0)
            {
                existingItem.Quantity += quantity;
            }

            await dbContext.SaveChangesAsync();
        }  

        public async Task<ShoppingCart> GetShoppingCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new ShoppingCart();
            
            return await dbContext.ShoppingCarts
                                  .Include(x => x.CartItems)
                                  .ThenInclude(x => x.Product)
                                  .FirstOrDefaultAsync(x => x.UserId == userId);
        }     
    }
}