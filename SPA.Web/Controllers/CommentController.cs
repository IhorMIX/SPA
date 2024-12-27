using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPA.BLL.Models;
using SPA.BLL.Services.Interfaces;
using SPA.Web.Models;

namespace SPA.Web.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class CommentController(IUserService userService, ICommentService commentService, IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CommentCreateModel comment,
        CancellationToken cancellationToken)
    {
        var commentModel = mapper.Map<CommentModel>(comment);
        var createdComment = await commentService.AddCommentAsync(commentModel, cancellationToken);
        var commentViewModel = mapper.Map<CommentViewModel>(createdComment);
        return Ok(commentViewModel);
    }
    
    // [HttpDelete("{userId:int}")]
    // public async Task<IActionResult> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    // {
    //     await userService.DeleteUserAsync(userId, cancellationToken);
    //     return Ok("User was deleted");
    // }
}
