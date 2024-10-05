using Api.Models;
using Api.ModelsDto;
using Api.Service;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class OrderController(AppDbContext dbContext, OrderService orderService) : StoreController(dbContext)
    {
        [HttpPost]
        public async Task<ActionResult<ResponseServer>> CreateOrder([FromBody] OrderHeaderCreateDto order)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseServer
                {
                    isSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = { "Неверная модель заказа" }
                });

            
            try
            {
                await orderService.CreateOrderAsync(order);
                order.OrderDetails = null;

                return Ok(new ResponseServer{
                    Result = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer
                {
                    isSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то пошло не так", ex.Message}
                });
            }
        }
    }
}