using Api.Common;
using Api.Models;
using Api.ModelsDto;
using Api.Service;
using Api.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Api.Controllers
{
    public class AuthController : StoreController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JwtTokenGenerator tokenGenerator;

        public AuthController(
            AppDbContext dbContext, 
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            JwtTokenGenerator tokenGenerator)  : base(dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto registerRequestDto
        )
        {
            try
            {
                if (registerRequestDto is null)
                return BadRequest(new ResponseServer(){
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Некорректная модель запроса" }
                });

                if (ModelState.IsValid)
                {
                    var userFromDb = await database.AppUsers
                        .FirstOrDefaultAsync(u => u.UserName.ToLower() == registerRequestDto.UserName.ToLower());

                    if (userFromDb is not null)
                        return BadRequest(new ResponseServer(){
                            isSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { "Пользователь с таким именем уже существует" }
                        });

                    if (await database.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == registerRequestDto.Email.ToLower()) is not null)
                        return BadRequest(new ResponseServer(){
                            isSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { "Пользователь с таким именем уже существует" }
                        });

                    var newAppUser = new AppUser
                    {
                        UserName = registerRequestDto.UserName,
                        Email = registerRequestDto.Email,
                        FirstName = registerRequestDto.UserName
                    };

                    var result = await userManager.CreateAsync(newAppUser, registerRequestDto.Password);

                    if (!result.Succeeded)
                        return BadRequest(new ResponseServer(){
                                isSuccess = false,
                                StatusCode = HttpStatusCode.BadRequest,
                                ErrorMessages = { "Ошибка регистрации" }
                            });
                    
                    var newRoleUser = registerRequestDto.Role.Equals(
                        SharedData.Roles.Admin, StringComparison.OrdinalIgnoreCase)
                        ? SharedData.Roles.Admin
                        : SharedData.Roles.Consumer;

                    await userManager.AddToRoleAsync(newAppUser, newRoleUser);

                    return Ok(new ResponseServer
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        Result = "Регистрация завершена"
                    });   
                }
                return BadRequest(new ResponseServer(){
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Ошибка регистрации" }
                });         
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseServer(){
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }         
        }


        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var userFromDb = await database.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());
            
            if (userFromDb is null || ! await userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password))
                return BadRequest(new ResponseServer 
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Такого пользователя не существует или неправильная пара логин и пароль" }
                });
            
            var roles = await userManager.GetRolesAsync(userFromDb);
            var token = tokenGenerator.GenerateJwtToken(userFromDb, roles);

            return Ok(new LoginResponseDto {
                Email = userFromDb.Email,
                Token = token,
            });
        }
    }
}