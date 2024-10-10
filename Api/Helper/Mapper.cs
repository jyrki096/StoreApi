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

        public static void UpdateToOrderDb(OrderHeaderUpdateDto orderHeaderUpdateDto, OrderHeader orderHeader)
        {
            orderHeader.CustomerEmail = string.IsNullOrEmpty(orderHeaderUpdateDto.CustomerEmail) ? orderHeader.CustomerEmail : orderHeaderUpdateDto.CustomerEmail;
            orderHeader.CustomerName = string.IsNullOrEmpty(orderHeaderUpdateDto.CustomerName) ? orderHeader.CustomerName : orderHeaderUpdateDto.CustomerName;
            orderHeader.Status = string.IsNullOrEmpty(orderHeaderUpdateDto.Status.ToString()) ? orderHeader.Status : orderHeaderUpdateDto.Status;
        }
    }
}
