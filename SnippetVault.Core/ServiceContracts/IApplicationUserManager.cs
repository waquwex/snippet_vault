using System.Security.Claims;

namespace SnippetVault.Core.ServiceContracts
{
    public interface IApplicationUserManager
    {
        public Guid GetUserGuid(ClaimsPrincipal user);

        Task<int> GetSnippetsCount(ClaimsPrincipal user);

        Task<int> GetStarsCount(ClaimsPrincipal user);

        Task<int> GetCommentsCount(ClaimsPrincipal user);

        Task<int> GetCommentLikesCount(ClaimsPrincipal user);

        Task<TimeSpan> GetEmailConfirmTimeDiff(Guid userId);

        Task UpdateEmailConfirmSentDate(Guid userId);
    }
}