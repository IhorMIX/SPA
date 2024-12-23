using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPA.BLL.Services.Interfaces;
using SPA.Web.Models;

namespace SPA.Web.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateModel user,CancellationToken cancellationToken)
    {
        var userModel = await userService.CreateUserAsync(user.UserName, user.Email, user.HomePage, cancellationToken);
        return Ok(mapper.Map<UserViewModel>(userModel));
    }
    
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(userId, cancellationToken);
        return Ok("User was deleted");
    }
}