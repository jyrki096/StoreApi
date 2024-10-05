using Api.Models;
using Api.ModelsDto;
using Api.Storage;

namespace Api.Service
{
    public class OrderService(AppDbContext dbContext)
    {
        public async Task<OrderHeader> CreateOrderAsync(OrderHeaderCreateDto orderHeaderCreateDto)
        {
            var order = new OrderHeader
            {
                AppUserId = orderHeaderCreateDto.UserId,
                CustomerName = orderHeaderCreateDto.CustomerName,
                CustomerEmail = orderHeaderCreateDto.CustomerEmail,
                OrderDateTime  = DateTime.UtcNow,
                TotalPrice = orderHeaderCreateDto.TotalPrice,
                TotalCount = orderHeaderCreateDto.TotalCount,
                Status = string.IsNullOrEmpty(orderHeaderCreateDto.Status) ? 
                                                          orderHeaderCreateDto.Status : 
                                                          "Nulllll"
            };

            await dbContext.OrderHeaders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            
            foreach (var details in orderHeaderCreateDto.OrderDetails)
            {
                var orderDetails = new OrderDetails
                {
                    OrderHeaderId = order.OrderHeaderId,
                    ProductId = details.ProductId,
                    Quantity = details.Quantity,
                    ItemName = details.ItemName,
                    Price = details.Price
                };

                await dbContext.OrderDetails.AddAsync(orderDetails);
            }

            await dbContext.SaveChangesAsync();

            return order;
        }
    }
}