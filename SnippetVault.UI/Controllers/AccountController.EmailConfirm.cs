using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.DTO.ApplicationUserDTOs;
using SnippetVault.UI.Filters.AuthorizationFilters;
using System.Text;

namespace SnippetVault.UI.Controllers
{
    public partial class AccountController
    {
        [TypeFilter(typeof(NotAuthorizedFilter))]
        [TypeFilter(typeof(RequireEmailConfirmSettingFilter))]
        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public IActionResult EmailNotConfirmed()
        {
            // We can redirect user to another page if sesion variable does not exists
            // but this might be useful
            var flexLoginName = HttpContext.Session.GetString("NotConfirmedEmailFlex");

            var model = new EmailNotConfirmedDTO();
            ViewBag.HaveSessionFlexName = flexLoginName != null;
            return View(model);
        }

        [TypeFilter(typeof(NotAuthorizedFilter))]
        [TypeFilter(typeof(RequireEmailConfirmSettingFilter))]
        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> EmailNotConfirmed([FromForm] EmailNotConfirmedDTO emailNotConfirmedDTO)
        {
            var flexLoginName = HttpContext.Session.GetString("NotConfirmedEmailFlex");
            if (flexLoginName == null)
            {
                // If there is error show errors in View
                if (!ModelState.IsValid)
                {
                    return View(emailNotConfirmedDTO);
                }
            }

            string? email = null;
            string? userName = null;

            // User gets here organically
            if (emailNotConfirmedDTO.Email == null)
            {
                if (flexLoginName == null)
                {
                    throw new Exception("Unexpected error!");
                }

                if (flexLoginName.Contains("@"))
                {
                    email = flexLoginName;
                }
                else
                {
                    userName = flexLoginName;
                }
            }
            else // User gets here by typing URL
            {
                email = emailNotConfirmedDTO.Email;
            }

            ApplicationUser user;

            if (email == null)
            {
                if (userName == null)
                {
                    throw new InvalidOperationException("Unexpected error!");
                }

                user = await _userManager.FindByNameAsync(userName);
            }
            else
            {
                user = await _userManager.FindByEmailAsync(email);
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new Exception("Email is already confirmed!");
            }

            try
            {
                var remainingTimeSpan = await SendConfirmationEmail(user);

                if (remainingTimeSpan == TimeSpan.Zero)
                {
                    ViewBag.EmailSent = true;
                    HttpContext.Session.Remove("NotConfirmedEmailFlex");
                }
                else
                {
                    ViewBag.EmailSent = false;
                    ViewBag.RemainingTime = remainingTimeSpan;
                }
            }
            catch (Exception ex)
            {
                ViewBag.EmailSent = false;
                HttpContext.Session.Remove("NotConfirmedEmailFlex");
            }

            return View();
        }

        [TypeFilter(typeof(NotAuthorizedFilter))]
        [TypeFilter(typeof(RequireEmailConfirmSettingFilter))]
        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var alreadyConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (alreadyConfirmed)
            {
                return LocalRedirect("/");
            }

            // decode token
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            token = Encoding.UTF8.GetString(tokenDecodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                throw new Exception("Can't confirm!");
            }

            return View();
        }
    }
}
