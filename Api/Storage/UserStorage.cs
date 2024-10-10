using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Storage
{
    public class UserStorage(AppDbContext database)
    {
        public async Task<AppUser> GetUserAsync(string email) => await database.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        public async Task<AppUser> GetUserByIdAsync(string id) => await database.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
    }
}
