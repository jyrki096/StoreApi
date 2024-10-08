using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Storage
{
    public class CartStorage(AppDbContext database)
    {
        public async Task CreateNewCartAsync(string userId, int productId, int quantity)
        {
            var cart = new ShoppingCart
            {
                UserId = userId
            };

            await database.ShoppingCarts.AddAsync(cart);
            await database.SaveChangesAsync();

            var item = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                ShoppingCartId = cart.Id,
                Product = null
            };

            await database.CartItems.AddAsync(item);
            await database.SaveChangesAsync();
        }

        public async Task UpdateExistingCartAsync(ShoppingCart shoppingCart, int productId, int quantity)
        {
            var existingItem = shoppingCart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem is null && quantity > 0)
            {
                await database.CartItems.AddAsync(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = shoppingCart.Id
                });
            }

            if (existingItem is not null && existingItem.Quantity + quantity <= 0)
            {
                database.CartItems.Remove(existingItem);

                if (shoppingCart.CartItems.Count == 0)
                    database.ShoppingCarts.Remove(shoppingCart);
            }

            if (existingItem is not null && existingItem.Quantity + quantity > 0)
            {
                existingItem.Quantity += quantity;
            }

            await database.SaveChangesAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new ShoppingCart();

            return await database.ShoppingCarts
                                  .Include(x => x.CartItems)
                                  .ThenInclude(x => x.Product)
                                  .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> RemoveCartAsync(ShoppingCart shoppingCart)
        {
            if (await database.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == shoppingCart.Id) == null)
                return false;

            database.ShoppingCarts.Remove(shoppingCart);
            await database.SaveChangesAsync();
            return true;
        }
    }
}