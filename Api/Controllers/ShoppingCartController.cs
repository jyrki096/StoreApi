using System.Net;
using Api.Models;
using Api.Service;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Api.Controllers
{
    public class ShoppingCartController(AppDbContext dbContext, ShoppingCartService shoppingCartService) : StoreController(dbContext)
    {
        [HttpGet]
        public async Task<ActionResult<ServerResponse>> AppendOrUpdateItemInCartAsync(string userId, int productId, int quantity)
        {
            var existingProduct =  await database.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (existingProduct is null)
                return NotFound(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Товар не найден" }
                });

            var existingCart = await shoppingCartService.GetShoppingCartAsync(userId);

            if (existingCart is null)
                await shoppingCartService.CreateNewCartAsync(userId, productId, quantity);
            else
                await shoppingCartService.UpdateExistingCartAsync(existingCart, productId, quantity);
            
            return Ok(new ServerResponse{
                isSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse>> GetShoppingCart(string userId)
        {
            try
            {
                var cart = await shoppingCartService.GetShoppingCartAsync(userId);
                
                return Ok(new ServerResponse{
                    isSuccess = true,
                    Result =  cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Ошибка получения корзины" , ex.Message }
                });
            }
        }
    }
}