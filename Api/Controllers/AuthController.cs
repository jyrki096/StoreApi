using Api.Common;
using Api.Models;
using Api.ModelsDto;
using Api.Service;
using Api.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    public class AuthController(UserStorage userStorage, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, JwtTokenGenerator tokenGenerator) : StoreController
    {

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto registerRequestDto)
        {
            try
            {
                if (registerRequestDto is null)
                    return BadRequest(new ServerResponse
                    {
                        isSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Некорректная модель запроса" }
                    });

                if (ModelState.IsValid)
                {
                    var userFromDb = await userStorage.GetUserAsync(registerRequestDto.Email);

                    if (userFromDb is not null)
                        return BadRequest(new ServerResponse
                        {
                            isSuccess = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorMessages = { "Пользователь с таким Email уже существует" }
                        });

                    var newAppUser = new AppUser
                    {
                        UserName = registerRequestDto.UserName,
                        Email = registerRequestDto.Email,
                        FirstName = registerRequestDto.UserName
                    };

                    var result = await userManager.CreateAsync(newAppUser, registerRequestDto.Password);

                    if (!result.Succeeded)
                        return BadRequest(new ServerResponse
                            {
                                isSuccess = false,
                                StatusCode = HttpStatusCode.BadRequest,
                                ErrorMessages = { "Ошибка регистрации" }
                            });
                    
                    var newRoleUser = registerRequestDto.Role.Equals(
                        SharedData.Roles.Admin, StringComparison.OrdinalIgnoreCase)
                        ? SharedData.Roles.Admin
                        : SharedData.Roles.Consumer;

                    await userManager.AddToRoleAsync(newAppUser, newRoleUser);

                    return Ok(new ServerResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        Result = "Регистрация завершена"
                    });   
                }
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Ошибка регистрации" }
                });         
            }
            catch (Exception ex)
            {
                return BadRequest(new ServerResponse
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Что-то поламалось", ex.Message }
                });
            }         
        }


        [HttpPost]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            var userFromDb = await userStorage.GetUserAsync(loginRequestDto.Email);
            
            if (userFromDb is null || ! await userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password))
                return BadRequest(new ServerResponse 
                {
                    isSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = { "Такого пользователя не существует или неправильная пара логин и пароль" }
                });
            
            var roles = await userManager.GetRolesAsync(userFromDb);
            var token = tokenGenerator.GenerateJwtToken(userFromDb, roles);

            return Ok(new LoginResponseDto 
            {
                Email = userFromDb.Email,
                Token = token,
            });
        }
    }
}