using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SnippetVault.UI.Controllers
{
    public partial class AccountController
    {
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAccount()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _userManager.DeleteAsync(currentUser);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Snippets");
        }
    }
}