using System.Net;
using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    public class CartController(ProductStorage productStorage, CartStorage cartStorage) : StoreController
    {
        [HttpGet]
        public async Task<ActionResult<ServerResponse>> AppendOrUpdateItemInCartAsync(string userId, int productId, int quantity)
        {
            var existingProduct =  await productStorage.GetProductByIdAsync(productId);

            if (existingProduct is null)
                return NotFound(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Товар не найден" }
                });

            var existingCart = await cartStorage.GetShoppingCartAsync(userId);

            if (existingCart is null)
                await cartStorage.CreateNewCartAsync(userId, productId, quantity);
            else
                await cartStorage.UpdateExistingCartAsync(existingCart, productId, quantity);
            
            return Ok(new ServerResponse
            {
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse>> GetCartAsync(string userId)
        {
            try
            {
                var cart = await cartStorage.GetShoppingCartAsync(userId);
                
                return Ok(new ServerResponse
                {
                    StatusCode= HttpStatusCode.OK,
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