using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var createdComment = await commentService.AddCommentAsync(comment.Text, comment.ParentCommentId, comment.UserId, cancellationToken);
        var commentViewModel = mapper.Map<CommentViewModel>(createdComment);
        return Ok(commentViewModel);
    }
    
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteCommentAsync(int commentId, CancellationToken cancellationToken)
    {
        await commentService.DeleteCommentAsync(commentId, cancellationToken);
        return Ok("Comment was deleted");
    }
    
    [HttpGet("{commentId}/replies")]
    public async Task<IActionResult> GetRepliesAsync(int commentId, CancellationToken cancellationToken)
    {
        var replies = await commentService.GetRepliesAsync(commentId, cancellationToken);
        var replyViewModels = mapper.Map<IEnumerable<CommentViewModel>>(replies);
        return Ok(replyViewModels);
    }
    
    [HttpGet("{commentId}/tree")]
    public async Task<IActionResult> GetCommentsTreeAsync(int commentId, CancellationToken cancellationToken)
    {
        var commentTree = await commentService.GetCommentsTreeAsync(commentId, cancellationToken);
        var commentTreeViewModel = mapper.Map<IEnumerable<CommentViewModel>>(commentTree);
        return Ok(commentTreeViewModel);
    }
}
