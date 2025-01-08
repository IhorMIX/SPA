using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPA.BLL.Models;
using SPA.BLL.Services.Interfaces;
using SPA.Web.Extensions;
using SPA.Web.Models;

namespace SPA.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper) : ControllerBase
{
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateViewModel user,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Start to create user");
        await userService.CreateUserAsync(mapper.Map<UserModel>(user), cancellationToken);

        logger.LogInformation("User was created");
        return Ok();
    }
    
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(userId, cancellationToken);
        return Ok("User was deleted");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserAsync(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId(); 
        var user = await userService.GetByIdAsync(userId, cancellationToken);
        var viewModel = mapper.Map<UserViewModel>(user);
        return Ok(viewModel);
    }
}