using System.Net;
using Api.ModelsDto;
using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Helper;

namespace Api.Controllers
{
    public class ProductController(AppDbContext appDb) : StoreController(appDb)
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(new ServerResponse
            { 
                StatusCode = HttpStatusCode.OK,
                Result = await database.Products.ToListAsync()
            });
        } 

        [HttpGet("{id}", Name = nameof(GetProductById))]
        public async Task<ActionResult<ServerResponse>> GetProductById(int id)
        {
            if (id <= 0 || id.GetType() != typeof(int))
                return BadRequest(new ServerResponse
                { 
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Некорректный id" }
                });

            var product = await database.Products.FirstOrDefaultAsync(x => x.Id == id);

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
        public async Task<ActionResult<ServerResponse>> CreateProduct([FromBody] CreatedProductDto createdProduct)
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

                    await database.Products.AddAsync(product);
                    await database.SaveChangesAsync();

                    return CreatedAtRoute(
                        nameof(GetProductById), 
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

        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        public async Task<ActionResult<ServerResponse>> UpdateProduct(int id, [FromBody] UpdatedProductDto updatedProductDto)
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

                    var product = await database.Products.FirstOrDefaultAsync(x => x.Id == id);

                    if (product is null)
                        return NotFound(new ServerResponse
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            isSuccess = false,
                            ErrorMessages = { "Товар с указанным id не найден" }
                        });

                    Mapper.UpdateToProductDb(updatedProductDto, product);

                    database.Products.Update(product);
                    await database.SaveChangesAsync();

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

        [HttpDelete("{id}", Name = nameof(DeleteProductById))]
        public async Task<ActionResult<ServerResponse>> DeleteProductById(int id)
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

                var product = await database.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product is null)
                    return NotFound(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Товар с указанным id не найден" }
                    });
             
                database.Remove(product);
                await database.SaveChangesAsync();

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