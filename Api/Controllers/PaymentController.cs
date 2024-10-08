using Api.Models;
using Api.Service.Payment;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    public class PaymentController(AppDbContext database, IPaymentService paymentService) : StoreController(database)
    {
        [HttpPost]
        public async Task<ActionResult<ServerResponse>> MakePayment(string userId, int orderHeaderId, string cardNumber)
        {
            try
            {
                return await paymentService.HandlePaymentAsync(userId, orderHeaderId, cardNumber);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то пошло не так", ex.Message }
                });
            }
        }
    }
}
