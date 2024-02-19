using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.ServiceContracts;
using SnippetVault.Core.Services;
using System.Text;

enum LoginStatus
{
    SUCCESS,
    FAILURE,
    EMAIL_NOT_CONFIRMED
}

namespace SnippetVault.UI.Controllers
{
    [Route("[controller]")]
    public partial class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AccountController(ApplicationUserManager userManager, IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public async Task<TimeSpan> SendConfirmationEmail(ApplicationUser user)
        {
            var timeDiff = await _userManager.GetEmailConfirmTimeDiff(user.Id);
            var remainingTime = new TimeSpan(0, 10, 0) - timeDiff;

            if (remainingTime <= TimeSpan.Zero)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
                token = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var confirmUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/account/confirmemail";
                await _emailService.SendConfirmationEmail(confirmUrl, user.Email, token);
                var userId = await _userManager.GetUserIdAsync(user);
                await _userManager.UpdateEmailConfirmSentDate(Guid.Parse(userId));

                return TimeSpan.Zero;
            }
            else
            {
                return remainingTime;
            }
        }
    }
}
