using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.ServiceContracts;

namespace SnippetVault.UI.Filters.ActionFilters
{
    public class AuthorizeCommentOwner : IAsyncActionFilter
    {
        private readonly ICommentService _commentService;
        private readonly IApplicationUserManager _userManager;

        public AuthorizeCommentOwner(ICommentService commentService, IApplicationUserManager userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as ControllerBase;

            if (controller.User.Identity == null)
            {
                context.Result = new StatusCodeResult(401);
            }

            if (!controller.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
            }

            if (context.ActionArguments.ContainsKey("commentId"))
            {
                var commentId = (Guid)context.ActionArguments["commentId"];
                var comment = await _commentService.GetCommentById(commentId);

                if (comment.CommentOwnerUserId != _userManager.GetUserGuid(controller.User))
                {
                    context.Result = new StatusCodeResult(401);
                }
                else
                {
                    await next();
                }
            }
            else if (context.ActionArguments.ContainsKey("commentUpdateRequest"))
            {
                var commentUpdateRequest = (CommentUpdateRequest)context.ActionArguments["commentUpdateRequest"];
                var comment = await _commentService.GetCommentById(commentUpdateRequest.CommentId.Value);

                if (comment.CommentOwnerUserId != _userManager.GetUserGuid(controller.User))
                {
                    context.Result = new StatusCodeResult(401);
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
