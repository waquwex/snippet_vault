using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Infrastructure.DbContext;

namespace SnippetVault.Infrastructure.Repositories
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>, IApplicationUserStore
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {
            _applicationDbContext = context;
        }

        public async Task WipeUserData(Guid userId)
        {
            _applicationDbContext.CommentLikes.RemoveRange(_applicationDbContext.CommentLikes.Where(el => el.OwnerUserId == userId));
            _applicationDbContext.Stars.RemoveRange(_applicationDbContext.Stars.Where(el => el.OwnerUserId == userId));
            await _applicationDbContext.SaveChangesAsync();

            _applicationDbContext.Comments.RemoveRange(_applicationDbContext.Comments.Where(el => el.OwnerUserId == userId));
            await _applicationDbContext.SaveChangesAsync();

            _applicationDbContext.Snippets.RemoveRange(_applicationDbContext.Snippets.Where(el => el.OwnerUserId == userId));
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> GetSnippetsCount(Guid userId)
        {
            return await _applicationDbContext.Snippets.CountAsync(s => s.OwnerUserId == userId);
        }

        public async Task<int> GetStarsCount(Guid userId)
        {
            return await _applicationDbContext.Stars.CountAsync(st => st.OwnerUserId == userId);
        }

        public async Task<int> GetCommentsCount(Guid userId)
        {
            return await _applicationDbContext.Comments.CountAsync(c => c.OwnerUserId == userId);
        }

        public async Task<int> GetCommentLikesCount(Guid userId)
        {
            return await _applicationDbContext.CommentLikes.CountAsync(cl => cl.OwnerUserId == userId);
        }

        public async Task<DateTime> GetLastEmailConfirmSent(Guid userId)
        {
            var user = await _applicationDbContext.Users.FirstAsync(us => us.Id == userId);
            return user.LastSentConfirmEmailTime;
        }

        public async Task UpdateLastEmailConfirmSent(Guid userId)
        {
            var user = await _applicationDbContext.Users.FirstAsync(us => us.Id == userId);
            user.LastSentConfirmEmailTime = DateTime.UtcNow;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}