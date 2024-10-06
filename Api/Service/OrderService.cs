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

        public async Task<bool> UpdateOrderByIdAsync(int orderId, OrderHeaderUpdateDto orderHeaderUpdateDto)
        {
            if (orderHeaderUpdateDto is null || orderHeaderUpdateDto.OrderHeaderId != orderId)
                return false;

            var orderHeader = await dbContext.OrderHeaders.FirstOrDefaultAsync(o => o.OrderHeaderId == orderId);

            if (orderHeader is null)
                return false;

            orderHeader.CustomerEmail = string.IsNullOrEmpty(orderHeaderUpdateDto.CustomerEmail) ? orderHeader.CustomerEmail : orderHeaderUpdateDto.CustomerEmail;
            orderHeader.CustomerName = string.IsNullOrEmpty(orderHeaderUpdateDto.CustomerName) ? orderHeader.CustomerName : orderHeaderUpdateDto.CustomerName;
            orderHeader.Status = string.IsNullOrEmpty(orderHeaderUpdateDto.Status.ToString()) ? orderHeader.Status : orderHeaderUpdateDto.Status;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}