
using System.Net;
using Api.Common;
using Api.Models;
using Api.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AuthTestController(AppDbContext dbContext) : StoreController(dbContext)
    {
        [HttpGet]
        public IActionResult Test1() => Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test1: для всех"
        });

        [HttpGet]
        [Authorize]
        public IActionResult Test2() => Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test2: для авторизованных"
        });

        [HttpGet]
        [Authorize(Roles = SharedData.Roles.Consumer)]
        public IActionResult Test3() => Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test1: для авторизованных пользователей с правами Consumer"
        });

        [HttpGet]
        [Authorize(Roles = SharedData.Roles.Admin)]
        public IActionResult Test4() => Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test1: для авторизованных пользователей с правами Admin"
        });

    }
}