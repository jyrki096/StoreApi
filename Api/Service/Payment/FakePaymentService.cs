using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Service.Payment
{
    public class FakePaymentService(OrderStorage orderStorage) : IPaymentService
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

            var order = await orderStorage.GetOrderByIdAsync(orderHeaderId);


            if (order is null)
                return new BadRequestObjectResult(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Такого заказа не существует" }
                    });
                

            var totalPrice = order.TotalPrice;

            await Task.Delay(5000);

            PaymentResponse paymentResponse;

            if (cardNumber == "1111 2222 3333 4444")
            {
                paymentResponse = new PaymentResponse
                {
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
            await orderStorage.SavaChangesInDbAsync();

            return new OkObjectResult(new ServerResponse
            {
                isSuccess = true,
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
