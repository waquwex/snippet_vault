using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SnippetVault.Core.DTO.ApplicationUserDTOs;
using SnippetVault.UI.Filters.AuthorizationFilters;
using System.Security.Policy;

namespace SnippetVault.UI.Controllers
{
    public partial class AccountController
    {
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        [TypeFilter(typeof(NotAuthorizedFilter))]
        public IActionResult Login([FromQuery] string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(NotAuthorizedFilter))]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO, [FromQuery] string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("UserName"))
                {
                    foreach (var userNameError in ModelState["UserName"].Errors)
                    {
                        ModelState["FlexLoginName"].Errors.Add(userNameError);
                    }
                }
                if (ModelState.ContainsKey("Email"))
                {
                    foreach (var emailError in ModelState["Email"].Errors)
                    {
                        ModelState["FlexLoginName"].Errors.Add(emailError);
                    }
                }

                return View(loginDTO);
            }

            var loginSuccess = LoginStatus.FAILURE;

            // Prioritize entered user name
            if (loginDTO.UserName != null)
            {
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, loginDTO.RememberMe, false);

                    if (result.Succeeded)
                    {
                        loginSuccess = LoginStatus.SUCCESS;
                    }
                    else if (_configuration.GetValue<bool>("EmailConfirmRequired") && result.IsNotAllowed)
                    {
                        var credentialsAreCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                        if (credentialsAreCorrect && !(await _userManager.IsEmailConfirmedAsync(user)))
                        {
                            loginSuccess = LoginStatus.EMAIL_NOT_CONFIRMED;
                        }
                    }
                }
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDTO.Password, loginDTO.RememberMe, false);
                    if (result.Succeeded)
                    {
                        loginSuccess = LoginStatus.SUCCESS;
                    }
                    else if (result.IsNotAllowed)
                    {
                        var credentialsAreCorrect = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                        if (credentialsAreCorrect && !(await _userManager.IsEmailConfirmedAsync(user)))
                        {
                            loginSuccess = LoginStatus.EMAIL_NOT_CONFIRMED;
                        }
                    }
                }
            }

            if (loginSuccess == LoginStatus.SUCCESS)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return LocalRedirect("/");
                }
            }
            else if (loginSuccess == LoginStatus.EMAIL_NOT_CONFIRMED)
            {
                HttpContext.Session.SetString("NotConfirmedEmailFlex", loginDTO.FlexLoginName);
                return RedirectToAction(nameof(EmailNotConfirmed)); //
            }
            else if (loginSuccess == LoginStatus.FAILURE)
            {
                if (loginDTO.UserName != null)
                {
                    var errorMessage = "Invalid username or password";
                    ModelState["FlexLoginName"].Errors.Add(errorMessage);
                    ModelState["Password"].Errors.Add(errorMessage);
                }
                else
                {
                    var errorMessage = "Invalid email or password";
                    ModelState["FlexLoginName"].Errors.Add(errorMessage);
                    ModelState["Password"].Errors.Add(errorMessage);
                }

                HttpContext.Session.Remove("NotConfirmedEmailFlex");
            }

            return View(loginDTO);
        }


        [Authorize]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/");
        }
    }
}