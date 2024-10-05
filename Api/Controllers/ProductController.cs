using System.Net;
using Api.ModelsDto;
using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ProductController(AppDbContext appDb) : StoreController(appDb)
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(new ResponseServer()
            { 
                StatusCode = HttpStatusCode.OK,
                Result = await database.Products.ToListAsync()
            });
        } 

        [HttpGet("{id}", Name = nameof(GetProductById))]
        public async Task<ActionResult<ResponseServer>> GetProductById(int id)
        {
            if (id <= 0 || id.GetType() != typeof(int))
                return BadRequest(new ResponseServer()
            { 
                StatusCode = HttpStatusCode.BadRequest,
                isSuccess = false,
                ErrorMessages = { "Некорректный id" }
            });

            var product = await database.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                return NotFound(new ResponseServer()
            { 
                StatusCode = HttpStatusCode.NotFound,
                isSuccess = false,
                ErrorMessages = { "Товар с указанным id не найден" }
            });

            return Ok(new ResponseServer()
            { 
                StatusCode = HttpStatusCode.OK,
                Result = product
            });
        } 

        [HttpPost]
        public async Task<ActionResult<ResponseServer>> CreateProduct([FromBody] CreatedProductDto createdProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = new Product(){
                        Name = createdProduct.Name,
                        SpecialTag = createdProduct.SpecialTag,
                        Category = createdProduct.Category,
                        Description = createdProduct.Description,
                        Price = createdProduct.Price,
                        Image = createdProduct.Image
                    };

                    await database.Products.AddAsync(product);
                    await database.SaveChangesAsync();

                    var response = new ResponseServer()
                    {
                        StatusCode = HttpStatusCode.Created,
                        Result = product
                    };
                    return CreatedAtRoute(nameof(GetProductById), new { id = product.Id }, response);
                }
                return BadRequest(new ResponseServer(){
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    ErrorMessages = { "Неверная модель" }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer(){
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }
        } 

        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        public async Task<ActionResult<ResponseServer>> UpdateProduct(int id, [FromBody] UpdatedProductDto updatedProductDto)
        {
            try
            {
                if (ModelState.IsValid)
                {            
                    if (updatedProductDto is null || id != updatedProductDto.Id)
                        return BadRequest(new ResponseServer()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            isSuccess = false,
                            ErrorMessages = { "Неверная модель" }
                        });

                    var product = await database.Products.FindAsync(id);

                    if (product is null)
                        return NotFound(new ResponseServer()
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            isSuccess = false,
                            ErrorMessages = { "Товар с указанным id не найден" }
                        });

                    product.Name = updatedProductDto.Name;
                    product.Description = updatedProductDto.Description;
                    product.SpecialTag = string.IsNullOrEmpty(updatedProductDto.SpecialTag) ? product.SpecialTag : updatedProductDto.SpecialTag;
                    product.Category = string.IsNullOrEmpty(updatedProductDto.Category) ? product.Category : updatedProductDto.Category;
                    product.Price = updatedProductDto.Price;
                    product.Image = string.IsNullOrEmpty(updatedProductDto.Image) ? product.Image : updatedProductDto.Image;

                    database.Products.Update(product);
                    await database.SaveChangesAsync();

                    return Ok(new ResponseServer()
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        Result = product
                    });
                }

                return BadRequest(new ResponseServer()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    ErrorMessages = { "Неверная модель" }
                });       
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer(){
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }
        } 

        [HttpDelete("{id}", Name = nameof(DeleteProductById))]
        public async Task<ActionResult<ResponseServer>> DeleteProductById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ResponseServer()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        isSuccess = false,
                        ErrorMessages = { "Некорректный id" }
                    });

                var product = await database.Products.FindAsync(id);

                if (product is null)
                    return NotFound(new ResponseServer()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        isSuccess = false,
                        ErrorMessages = { "Товар с указанным id не найден" }
                    });
             
                database.Remove(product);
                await database.SaveChangesAsync();

                return Ok(new ResponseServer()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer(){
                    StatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }
        }
    }
}