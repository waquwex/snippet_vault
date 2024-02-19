using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.UI.Filters.ActionFilters;

namespace SnippetVault.UI.Controllers
{
    public partial class SnippetsController
    {
        [Authorize]
        [TypeFilter(typeof(AuthorizeSnippetOwner))]
        [Route("[action]/{snippetId}")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid snippetId)
        {
            await _snippetService.DeleteSnippetById(snippetId);
            return LocalRedirect("/snippets/mysnippets");
        }
    }
}
