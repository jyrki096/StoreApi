using Api.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class StoreController(AppDbContext dbContext) : ControllerBase
    {
        protected AppDbContext database = dbContext;
    }
}