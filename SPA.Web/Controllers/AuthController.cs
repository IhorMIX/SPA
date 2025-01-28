using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPA.BLL.Services.Interfaces;
using SPA.Web.Extensions;
using SPA.Web.Helpers;
using SPA.Web.Models;

namespace SPA.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly TokenHelper _tokenHelper;

    public AuthController(IUserService userService, TokenHelper tokenHelper)
    {
        _userService = userService;
        _tokenHelper = tokenHelper;
    }
   
    [AllowAnonymous]
    [HttpPost("token/{refreshToken}")]
    public async Task<IActionResult> UpdateTokenAsync([FromQuery] string refreshToken, CancellationToken cancellationToken)
    {
        refreshToken = refreshToken.Replace(" ", "+"); 
        var user = await _userService.GetUserByRefreshTokenAsync(refreshToken, cancellationToken);
        var token = _tokenHelper.GetToken(user.Id);
        return Ok(new { accessKey = token, refresh_token = refreshToken, expiredDate = user.AuthorizationInfo.ExpiredDate });
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> AuthorizeUser([FromBody] UserAuthorizeModel model, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByLoginAndPasswordAsync(model.UserName, model.Password, cancellationToken);
        var token = _tokenHelper.GetToken(user!.Id);
        var refreshToken = TokenHelper.GenerateRefreshToken(token);
        DateTime? expiredDate = model.IsNeedToRemember ? null : DateTime.Now;
        
        await _userService.AddAuthorizationValueAsync(
            user, 
            refreshToken, 
            expiredDate, 
            cancellationToken);
        
        return Ok(new { accessKey = token, refresh_token = refreshToken, expiredDate = expiredDate });
    }
    
    [HttpPost]
    [Route(("logout"))]
    public async Task<IActionResult> LogOutAsync(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        await _userService.LogOutAsync(userId, cancellationToken);
        return Ok();
    }
    
}