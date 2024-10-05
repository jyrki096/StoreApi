using Api.Models;
using Api.ModelsDto;
using Api.Storage;
using Microsoft.EntityFrameworkCore;

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
                Status = string.IsNullOrEmpty(orderHeaderCreateDto.Status.ToString()) ? 
                                                          OrderStatus.Created : 
                                                          orderHeaderCreateDto.Status
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

        public async Task<OrderHeader> GetOrderByIdAsync(int id)
        {
            return await dbContext.OrderHeaders
                                  .Include(items => items.OrderDetailsItems)
                                  .ThenInclude(p => p.Product)
                                  .FirstOrDefaultAsync(o => o.OrderHeaderId  == id);
        }

        public async Task<List<OrderHeader>> GetOrderByUserIdAsync(string userId)
        {
            return await dbContext.OrderHeaders
                            .Include(items => items.OrderDetailsItems)
                            .ThenInclude(p => p.Product)
                            .Where(u => u.AppUserId == userId)
                            .OrderBy(o => o.OrderHeaderId)
                            .ToListAsync();
        }
    }
}