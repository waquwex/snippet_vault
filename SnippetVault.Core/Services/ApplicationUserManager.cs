using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.ServiceContracts;
using System.Security.Claims;

namespace SnippetVault.Core.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        private readonly IApplicationUserStore _applicationUserStore;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger, IApplicationUserStore applicationUserStore)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _applicationUserStore = applicationUserStore;
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            await _applicationUserStore.WipeUserData(user.Id);
            return await base.DeleteAsync(user);
        }

        public Guid GetUserGuid(ClaimsPrincipal user)
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                throw new Exception("User doesn't have ClaimTypes.NameIdentifier. This should not happen.");
            }

            var userId = Guid.Parse(userIdString);
            return userId;
        }

        // @TODO: Convert ClaimsPrincipal to Guid: there is no logic regarding to multiple users
        public async Task<int> GetCommentLikesCount(ClaimsPrincipal user)
        {
            return await _applicationUserStore.GetCommentLikesCount(GetUserGuid(user));
        }

        public async Task<int> GetCommentsCount(ClaimsPrincipal user)
        {
            return await _applicationUserStore.GetCommentsCount(GetUserGuid(user));
        }

        public async Task<int> GetSnippetsCount(ClaimsPrincipal user)
        {
            return await _applicationUserStore.GetSnippetsCount(GetUserGuid(user));
        }

        public async Task<int> GetStarsCount(ClaimsPrincipal user)
        {
            return await _applicationUserStore.GetStarsCount(GetUserGuid(user));
        }

        public async Task<TimeSpan> GetEmailConfirmTimeDiff(Guid userId)
        {
            var datetime = await _applicationUserStore.GetLastEmailConfirmSent(userId);
            return DateTime.UtcNow - datetime;
        }

        public async Task UpdateEmailConfirmSentDate(Guid userId)
        {
            await _applicationUserStore.UpdateLastEmailConfirmSent(userId);
        }
    }
}