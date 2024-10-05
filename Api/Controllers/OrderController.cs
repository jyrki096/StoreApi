using Api.Models;
using Api.ModelsDto;
using Api.Service;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Неверная модель заказа" }
                });


            try
            {
                await orderService.CreateOrderAsync(order);
                order.OrderDetails = null;

                return Ok(new ResponseServer
                {
                    Result = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то пошло не так", ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseServer>> GetOrderById(int id)
        { 
            try
            {
                if (id <= 0)
                    return BadRequest(new ResponseServer
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Некорректный id" }
                    });

                var order = await orderService.GetOrderByIdAsync(id);

                if (order is null)
                    return NotFound(new ResponseServer
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Заказ с таким id не найден" }
                    });


                return Ok(new ResponseServer
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то пошло не так", ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseServer>> GetOrderByUserId(string id)
        {
            try
            {
                var isUserExist = await database.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (string.IsNullOrEmpty(id) || isUserExist is null)
                    return BadRequest(new ResponseServer
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Некорректный id или пользователя с таким id не существует" }
                    });

                var orders = await orderService.GetOrderByUserIdAsync(id);

                return Ok(new ResponseServer
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = orders
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseServer
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessages = { "При обработке запроса возникла ошибка", ex.Message }
                });
            }
        }
    }
}