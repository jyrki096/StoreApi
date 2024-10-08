using System.Net;
using Api.ModelsDto;
using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using Api.Helper;

namespace Api.Controllers
{
    public class ProductController(ProductStorage productStorage) : StoreController
    {
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            return Ok(new ServerResponse
            {
                StatusCode = HttpStatusCode.OK,
                Result = await productStorage.GetAllAsync()
            });
        } 

        [HttpGet("{id}", Name = nameof(GetProductByIdAsync))]
        public async Task<ActionResult<ServerResponse>> GetProductByIdAsync(int id)
        {
            if (id <= 0 || id.GetType() != typeof(int))
                return BadRequest(new ServerResponse
                { 
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Некорректный id" }
                });

            var product = await productStorage.GetProductByIdAsync(id);

            if (product is null)
                return NotFound(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Товар с указанным id не найден" }
                });

            return Ok(new ServerResponse
            { 
                StatusCode = HttpStatusCode.OK,
                Result = product
            });
        } 

        [HttpPost]
        public async Task<ActionResult<ServerResponse>> CreateProductAsync([FromBody] CreatedProductDto createdProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = new Product
                    {
                        Name = createdProduct.Name,
                        SpecialTag = createdProduct.SpecialTag,
                        Category = createdProduct.Category,
                        Description = createdProduct.Description,
                        Price = createdProduct.Price,
                        Image = createdProduct.Image
                    };

                    await productStorage.AddProductAsync(product);

                    return CreatedAtRoute(
                        nameof(GetProductByIdAsync), 
                        new { id = product.Id },
                        new ServerResponse
                        {
                            StatusCode = HttpStatusCode.Created,
                            Result = product
                        });
                }

                return BadRequest(new ServerResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    ErrorMessages = { "Неверная модель" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }
        } 

        [HttpPut("{id}", Name = nameof(UpdateProductAsync))]
        public async Task<ActionResult<ServerResponse>> UpdateProductAsync(int id, [FromBody] UpdatedProductDto updatedProductDto)
        {
            try
            {
                if (ModelState.IsValid)
                {            
                    if (updatedProductDto is null || id != updatedProductDto.Id)
                        return BadRequest(new ServerResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            isSuccess = false,
                            ErrorMessages = { "Неверная модель" }
                        });

                    var product = await productStorage.GetProductByIdAsync(id);

                    if (product is null)
                        return NotFound(new ServerResponse
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            isSuccess = false,
                            ErrorMessages = { "Товар с указанным id не найден" }
                        });

                    Mapper.UpdateToProductDb(updatedProductDto, product);
                    await productStorage.UpdateProductAsync(product);

                    return Ok(new ServerResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        Result = product
                    });
                }

                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Неверная модель" }
                });       
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }
        } 

        [HttpDelete("{id}", Name = nameof(DeleteProductByIdAsync))]
        public async Task<ActionResult<ServerResponse>> DeleteProductByIdAsync(int id)
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

                var isRemoved = await productStorage.RemoveByIdAsync(id);

                if (!isRemoved)
                    return NotFound(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Товар с указанным id не найден" }
                    });

                return Ok(new ServerResponse()
                {
                    StatusCode = HttpStatusCode.NoContent,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }
        }
    }
}