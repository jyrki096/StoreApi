using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Api.Service.Payment
{
    public class FakePaymentService(AppDbContext database) : IPaymentService
    {
        public async Task<ActionResult<ServerResponse>> HandlePaymentAsync(string userId, int orderHeaderId, string cardNumber)
        {
            if (userId is null || string.IsNullOrEmpty(userId) || orderHeaderId <= 0)
                return new BadRequestObjectResult(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Некорректный id" }
                });

            var cart = await database.ShoppingCarts
                                     .Include(x => x.CartItems)
                                     .ThenInclude(x => x.Product)
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            var order = await database.OrderHeaders.FirstOrDefaultAsync(o => o.OrderHeaderId == orderHeaderId);

            
            if (cart is null || cart.CartItems is null || cart.TotalAmount == 0)
                return new BadRequestObjectResult(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Корзина пуста или не найдена" }
                });

            if (order is null)
                return new BadRequestObjectResult(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Такого заказа не существует" }
                    });
                

            var totalPrice = cart.TotalPrice;

            await Task.Delay(5000);

            PaymentResponse paymentResponse;

            if (cardNumber == "1111 2222 3333 4444")
            {
                paymentResponse = new PaymentResponse
                {
                    Success =  true,
                    IntentId = Guid.NewGuid().ToString(),
                    Secret = Guid.NewGuid().ToString(),
                };
            }
            else
            {
                paymentResponse = new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = "ОШИБКА ОБРАБОТКИ ПЛАТЕЖА"
                };
            }

            

            if (!paymentResponse.Success)
                return new BadRequestObjectResult(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { paymentResponse.ErrorMessage }
                });

            order.Status = OrderStatus.Processed;
            await database.SaveChangesAsync();

            return new OkObjectResult(new ServerResponse
            {
                isSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
