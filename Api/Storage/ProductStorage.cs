using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Storage
{
    public class ProductStorage(AppDbContext database)
    {
        public async Task<IEnumerable<Product>> GetAllAsync() => await database.Products.ToListAsync();

        public async Task<Product> GetProductByIdAsync(int id) => await database.Products.FirstOrDefaultAsync(p => p.Id == id);
        public async Task AddProductAsync(Product product)
        {
            await database.Products.AddAsync(product);
            await database.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            database.Products.Update(product);
            await database.SaveChangesAsync();
        }

        public async Task<bool> RemoveByIdAsync(int id)
        {
            var product = await GetProductByIdAsync(id);

            if (product is null)
                return false;

            database.Remove(product);
            await database.SaveChangesAsync();

            return true;
        }
    }
}
