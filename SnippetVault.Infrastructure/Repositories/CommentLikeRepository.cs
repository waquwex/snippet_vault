using Microsoft.EntityFrameworkCore;
using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Infrastructure.DbContext;

namespace SnippetVault.Infrastructure.Repositories


{
    public class CommentLikeRepository : ICommentLikeRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CommentLikeRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<CommentLike> AddCommentLike(CommentLike commentLike)
        {
            await _applicationDbContext.CommentLikes.AddAsync(commentLike);
            await _applicationDbContext.SaveChangesAsync();
            return commentLike;
        }

        public async Task<bool> DeleteCommentLikeById(Guid commentLikeId)
        {
            _applicationDbContext.CommentLikes
                .RemoveRange(_applicationDbContext.CommentLikes.Where(el => el.CommentLikeId == commentLikeId));
            
            var rowsDeleted = await _applicationDbContext.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<CommentLike> GetCommentLikeById(Guid commentLikeId)
        {
            return await _applicationDbContext.CommentLikes.FirstAsync(el => el.CommentLikeId == commentLikeId);
        }

        public async Task<CommentLike?> GetCommentLikeByOwnerUserIdAndCommentId(Guid ownerUserId, Guid commentId)
        {
            return await _applicationDbContext.CommentLikes
                .FirstOrDefaultAsync(el => el.OwnerUserId == ownerUserId && el.CommentId == commentId);
        }

        public async Task<CommentLike> UpdateCommentLike(CommentLike commentLike)
        {
            var found = await _applicationDbContext.CommentLikes.FirstAsync(el => el.CommentLikeId == commentLike.CommentLikeId);
            found.CommentLikeSize = commentLike.CommentLikeSize;
            await _applicationDbContext.SaveChangesAsync();

            return found;
        }
    }
}