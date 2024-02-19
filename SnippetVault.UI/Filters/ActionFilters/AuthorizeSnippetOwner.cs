using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.ServiceContracts;
using System.Runtime.CompilerServices;

namespace SnippetVault.UI.Filters.ActionFilters
{
    public class AuthorizeSnippetOwner : IAsyncActionFilter
    {
        private readonly ISnippetService _snippetService;
        private readonly IApplicationUserManager _userManager;

        public AuthorizeSnippetOwner(ISnippetService snippetService, IApplicationUserManager userManager)
        {
            _snippetService = snippetService;
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

            if (context.ActionArguments.ContainsKey("snippetId"))
            {
                var snippetId = (Guid)context.ActionArguments["snippetId"];
                var snippet = await _snippetService.GetSnippetById(snippetId);

                if (snippet.SnippetOwnerUserId != _userManager.GetUserGuid(controller.User))
                {
                    context.Result = new StatusCodeResult(401);
                }
                else
                {
                    await next();
                }
            }
            else if (context.ActionArguments.ContainsKey("snippetUpdateRequest"))
            {
                var snippetUpdateRequest = (SnippetUpdateRequest)context.ActionArguments["snippetUpdateRequest"];
                var snippet = await _snippetService.GetSnippetById(snippetUpdateRequest.SnippetId.Value);

                if (snippet.SnippetOwnerUserId != _userManager.GetUserGuid(controller.User))
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
