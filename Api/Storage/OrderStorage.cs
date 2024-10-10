using Api.Helper;
using Api.Models;
using Api.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace Api.Storage
{
    public class OrderStorage(AppDbContext dbContext, ProductStorage productStorage)
    {
        public async Task<OrderHeader> CreateOrderAsync(OrderHeaderCreateDto orderHeaderCreateDto)
        {
            var order = new OrderHeader
            {
                AppUserId = orderHeaderCreateDto.UserId,
                CustomerName = orderHeaderCreateDto.CustomerName,
                CustomerEmail = orderHeaderCreateDto.CustomerEmail,
                OrderDateTime = DateTime.UtcNow.AddHours(3),
                TotalCount = orderHeaderCreateDto.OrderDetails.Sum(x => x.Quantity),
                Status = string.IsNullOrEmpty(orderHeaderCreateDto.Status.ToString()) || orderHeaderCreateDto.Status == OrderStatus.Created ?
                                                          OrderStatus.Created :
                                                          orderHeaderCreateDto.Status
            };

            await dbContext.OrderHeaders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var orderSum = (decimal)0;

            foreach (var details in orderHeaderCreateDto.OrderDetails)
            {
                var product = await productStorage.GetProductByIdAsync(details.ProductId);

                var orderDetails = new OrderDetails
                {
                    OrderHeaderId = order.OrderHeaderId,
                    ProductId = details.ProductId,
                    Quantity = details.Quantity,
                    ItemName = product.Name,
                    Price = product.Price
                };

                orderSum += product.Price;
                await dbContext.OrderDetails.AddAsync(orderDetails);
            }
            order.TotalPrice = orderSum;
            await dbContext.SaveChangesAsync();

            return order;
        }

        public async Task<OrderHeader> GetOrderByIdAsync(int id)
        {
            return await dbContext.OrderHeaders
                                  .Include(items => items.OrderDetailsItems)
                                  .ThenInclude(p => p.Product)
                                  .FirstOrDefaultAsync(o => o.OrderHeaderId == id);
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

            Mapper.UpdateToOrderDb(orderHeaderUpdateDto, orderHeader);

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task SavaChangesInDbAsync() => await dbContext.SaveChangesAsync();
    }
}