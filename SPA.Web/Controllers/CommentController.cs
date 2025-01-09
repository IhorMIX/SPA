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
public class CommentController(IUserService userService, ICommentService commentService, IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CommentCreateModel comment,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId(); 
        var createdComment = await commentService.AddCommentAsync(comment.Text, comment.ParentCommentId, userId, cancellationToken);
        var commentViewModel = mapper.Map<CommentViewModel>(createdComment);
        return Ok(commentViewModel);
    }
    
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteCommentAsync(int commentId, CancellationToken cancellationToken)
    {
        await commentService.DeleteCommentAsync(commentId, cancellationToken);
        return Ok("Comment was deleted");
    }
    
    // [HttpGet("{commentId}/replies")]
    // public async Task<IActionResult> GetRepliesAsync([FromQuery] PaginationModel pagination, int commentId, CancellationToken cancellationToken)
    // {
    //     var replies = await commentService.GetRepliesAsync(commentId,pagination, cancellationToken);
    //     var replyViewModels = mapper.Map<PaginationResultViewModel<CommentViewModel>>(replies);
    //     return Ok(replyViewModels);
    // }
    
    [HttpGet("{commentId}/tree")]
    public async Task<IActionResult> GetTreeByCommentIdAsync(int commentId, CancellationToken cancellationToken)
    {
        var commentTree = await commentService.GetTreeByCommentIdAsync(commentId, cancellationToken);
        var commentTreeViewModel = mapper.Map<IEnumerable<CommentViewModel>>(commentTree);
        return Ok(commentTreeViewModel);
    }
    
    [HttpGet("parent-comments")]
    public async Task<IActionResult> GetAllParentComments(
        [FromQuery] PaginationModel pagination,
        CancellationToken cancellationToken)
    {
        var result = await commentService.GetAllParentCommentsAsync(pagination, cancellationToken);
        var mapperModels = mapper.Map<PaginationResultModel<CommentViewModel>>(result);
        return Ok(mapperModels);
    }
    
    [HttpGet("comments/trees")]
    public async Task<IActionResult> GetAllCommentTrees([FromQuery] PaginationModel pagination, CancellationToken cancellationToken)
    {
        var commentTrees = await commentService.GetAllCommentTreesAsync(pagination, cancellationToken);
        var commentTreesViewModel = mapper.Map<PaginationResultModel<CommentViewModel>>(commentTrees);
        
        return Ok(commentTreesViewModel);
    }

}
