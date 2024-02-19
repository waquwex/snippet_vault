using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.ApplicationUserDTOs;
using SnippetVault.UI.ViewModels;
using System.Security.Claims;

namespace SnippetVault.UI.Controllers
{
    public partial class AccountController
    {

        [Authorize]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var updateAccountDTO = new UpdateAccountDTO()
            {
                UserName = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email)
            };

            var viewModel = new AccountDetailsViewModel()
            {
                UpdateAccountDTO = updateAccountDTO,
                SnippetsCount = await _userManager.GetSnippetsCount(User),
                StarsCount = await _userManager.GetStarsCount(User),
                CommentsCount = await _userManager.GetCommentsCount(User),
                CommentLikesCount = await _userManager.GetCommentLikesCount(User),
            };

            return View(viewModel);
        }

        [Authorize]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Details(AccountDetailsViewModel accountDetailsViewModel)
        {
            accountDetailsViewModel.SnippetsCount = await _userManager.GetSnippetsCount(User);
            accountDetailsViewModel.StarsCount = await _userManager.GetStarsCount(User);
            accountDetailsViewModel.CommentsCount = await _userManager.GetCommentsCount(User);
            accountDetailsViewModel.CommentLikesCount = await _userManager.GetCommentLikesCount(User);
            accountDetailsViewModel.UpdateAccountDTO.UserName = User.Identity.Name;
            accountDetailsViewModel.UpdateAccountDTO.Email = User.FindFirstValue(ClaimTypes.Email);


            if (!ModelState.IsValid)
            {
                return View(accountDetailsViewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception("User shouldn't be null");
            }


            var result = await _userManager.ChangePasswordAsync(user, accountDetailsViewModel.UpdateAccountDTO.CurrentPassword,
                accountDetailsViewModel.UpdateAccountDTO.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("UpdateAccountDTO.CurrentPassword", string.Join("\n", result.Errors.Select(e => e.Description)));
                return View(accountDetailsViewModel);
            }
            else
            {
                await _signInManager.SignOutAsync();

                return View("AccountUpdateSuccessful");
            }
        }
    }
}
