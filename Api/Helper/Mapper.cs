using Api.Models;
using Api.ModelsDto;

namespace Api.Helper
{
    public static class Mapper
    {
        public static void UpdateToProductDb(UpdatedProductDto updatedProductDto, Product product)
        {
            product.Name = string.IsNullOrEmpty(updatedProductDto.Name)? product.Name : updatedProductDto.Name;
            product.Description = string.IsNullOrEmpty(updatedProductDto.Description)? product.Description : updatedProductDto.Description;
            product.SpecialTag = string.IsNullOrEmpty(updatedProductDto.SpecialTag) ? product.SpecialTag : updatedProductDto.SpecialTag;
            product.Category = string.IsNullOrEmpty(updatedProductDto.Category) ? product.Category : updatedProductDto.Category;
            product.Price = updatedProductDto.Price == decimal.Zero? product.Price : updatedProductDto.Price;
            product.Image = string.IsNullOrEmpty(updatedProductDto.Image) ? product.Image : updatedProductDto.Image;
        }
    }
}
