using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using SnippetVault.Core.DTO.ApplicationUserDTOs;
using SnippetVault.UI.Filters.AuthorizationFilters;
using System.Text;

namespace SnippetVault.UI.Controllers
{
    public partial class AccountController
    {
        [TypeFilter(typeof(NotAuthorizedFilter))]
        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [TypeFilter(typeof(NotAuthorizedFilter))]
        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var userToCreate = registerDTO.ToApplicationUser();

            var result = await _userManager.CreateAsync(userToCreate, registerDTO.Password);

            var identityErrors = new Dictionary<string, string>();

            // Display Identity errors
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    identityErrors.Add(error.Code, error.Description);
                }

                ViewBag.IdentityErrors = identityErrors;
                return View(registerDTO);
            }


            // Email verification
            if (_configuration.GetValue<bool>("EmailConfirmRequired") == true)
            {
                await SendConfirmationEmail(userToCreate);
            }

            return RedirectToAction(nameof(RegistrationSuccess));
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public async Task<bool> IsEmailNotRegistered(string? email)
        {
            if (email.IsNullOrEmpty())
            {
                return false;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return true;
            }

            return false;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public async Task<bool> IsUserNameNotRegistered(string? userName)
        {
            if (userName.IsNullOrEmpty())
            {
                return false;
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return true;
            }

            return false;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> RegistrationSuccess()
        {
            ViewBag.EmailConfirmRequired = _configuration.GetValue<bool>("EmailConfirmRequired");

            return View();
        }
    }
}