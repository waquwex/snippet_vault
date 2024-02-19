using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.UI.Filters.ActionFilters;

namespace SnippetVault.UI.Controllers
{
    public partial class SnippetsController
    {

        [Authorize]
        [TypeFilter(typeof(AuthorizeSnippetOwner))]
        [Route("[action]/{snippetId}")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid snippetId)
        {
            var snippetResponse = await _snippetService.GetSnippetById(snippetId);
            var snippetUpdateRequest = snippetResponse.ToSnippetUpdateRequest();

            return View(snippetUpdateRequest);
        }

        [Authorize]
        [TypeFilter(typeof(AuthorizeSnippetOwner))]
        [Route("[action]/{snippetId}")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] SnippetUpdateRequest snippetUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(snippetUpdateRequest);
            }

            await _snippetService.UpdateSnippet(snippetUpdateRequest);

            return LocalRedirect("/snippets/" + snippetUpdateRequest.SnippetId);
        }
    }
}
