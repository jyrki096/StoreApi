using Api.Models;
using Api.ModelsDto;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    public class OrderController(UserStorage userStorage, OrderStorage orderStorage) : StoreController
    {
        [HttpPost]
        public async Task<ActionResult<ServerResponse>> CreateOrderAsync([FromBody] OrderHeaderCreateDto order)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Неверная модель заказа" }
                });


            try
            {
                await orderStorage.CreateOrderAsync(order);
                order.OrderDetails = null;

                return Ok(new ServerResponse
                {
                    Result = order
                });
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse>> GetOrderByIdAsync(int id)
        { 
            try
            {
                if (id <= 0)
                    return BadRequest(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Некорректный id" }
                    });

                var order = await orderStorage.GetOrderByIdAsync(id);

                if (order is null)
                    return NotFound(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Заказ с таким id не найден" }
                    });


                return Ok(new ServerResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = order
                });
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse>> GetOrderByUserIdAsync(string id)
        {
            try
            {
                var isUserExist = await userStorage.GetUserByIdAsync(id);

                if (string.IsNullOrEmpty(id) || isUserExist is null)
                    return BadRequest(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Некорректный id или пользователя с таким id не существует" }
                    });

                var orders = await orderStorage.GetOrderByUserIdAsync(id);

                return Ok(new ServerResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = orders
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessages = { "При обработке запроса возникла ошибка", ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerResponse>> UpdateOrderHeaderAsync(int id, [FromBody] OrderHeaderUpdateDto orderHeaderUpdateDto)
        {
            try
            {
                var result = await orderStorage.UpdateOrderByIdAsync(id, orderHeaderUpdateDto);

                if (!result)
                    return BadRequest(
                        new ServerResponse 
                        { 
                            isSuccess = false, 
                            StatusCode = HttpStatusCode.BadRequest, 
                            ErrorMessages = { "Во время обновления произошла ошибка" } 
                        });

                return Ok(
                    new ServerResponse 
                    { 
                        StatusCode = HttpStatusCode.OK, 
                        Result = "Всё обновлено" 
                    });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessages = { "При обработке запроса возникла ошибка", ex.Message }
                });
            }
        }
    }
}