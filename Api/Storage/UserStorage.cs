using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Storage
{
    public class UserStorage(AppDbContext database)
    {
        public async Task<AppUser> GetUserAsync(string email) => await database.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
