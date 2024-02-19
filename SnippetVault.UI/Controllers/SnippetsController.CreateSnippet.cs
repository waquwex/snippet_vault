using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.SnippetDTOs;

namespace SnippetVault.UI.Controllers
{
    public partial class SnippetsController
    {
        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateSnippet()
        {
            return View();
        }

        [Route("[action]")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSnippet([FromForm] SnippetAddRequest snippetAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(snippetAddRequest);
            }

            snippetAddRequest.SnippetOwnerUser = User;
            await _snippetService.AddSnippet(snippetAddRequest);

            return RedirectToAction(nameof(MySnippets));
        }
    }
}
