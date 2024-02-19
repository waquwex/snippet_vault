using Microsoft.EntityFrameworkCore;
using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.Helpers;
using SnippetVault.Infrastructure.DbContext;


namespace SnippetVault.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CommentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            await _applicationDbContext.Comments.AddAsync(comment);
            await _applicationDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentById(Guid commentId)
        {
            _applicationDbContext.Comments.RemoveRange(_applicationDbContext.Comments.Where(el => el.CommentId == commentId));
            var rowsDeleted = await _applicationDbContext.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<Comment> GetCommentById(Guid commentId)
        {
            var comment = await _applicationDbContext.Comments.FirstAsync(el => el.CommentId == commentId);
            return comment;
        }

        public async Task<ICollection<Comment>> GetOlderComments(QueryPivot? queryPivot, int size, Guid snippetId)
        {
            var descQuery = _applicationDbContext.Comments.Include(el => el.OwnerUser)
        .OrderByDescending(el => el.CommentCreatedDateTime).ThenByDescending(el => el.CommentId)
        .Where(el => el.Hidden == false && el.CommentSnippetId == snippetId);

            var comments = new List<Comment>();

            if (queryPivot == null)
            {
                comments = await descQuery.Take(size).ToListAsync();
            }
            else
            {
                comments = await descQuery.Where(el => el.CommentCreatedDateTime <= queryPivot.PivotDateTime && el.CommentId <= queryPivot.PivotId).Take(size).ToListAsync();

                if (comments.Count < size)
                {
                    var addToTopSize = size - comments.Count;

                    var ascQuery = _applicationDbContext.Comments.Include(el => el.OwnerUser)
                            .OrderBy(el => el.CommentCreatedDateTime).ThenBy(el => el.CommentId).Where(el => el.Hidden == false && el.CommentSnippetId == snippetId);

                    var topPartOfSnippets = await ascQuery.Where(el => el.CommentCreatedDateTime >= queryPivot.PivotDateTime && el.CommentId > queryPivot.PivotId)
                        .Take(addToTopSize).Reverse().ToListAsync();

                    topPartOfSnippets.AddRange(comments);
                    comments = topPartOfSnippets;
                }
            }

            foreach (var comment in comments)
            {
                comment.CommentLikesCount = 0;
            }

            return comments;
        }

        public async Task<ICollection<Comment>> GetNewerComments(QueryPivot queryPivot, int size, Guid snippetId)
        {
            var ascQuery = _applicationDbContext.Comments.Include(el => el.OwnerUser)
                .OrderBy(el => el.CommentCreatedDateTime).ThenBy(el => el.CommentId)
                .Where(el => el.Hidden == false && el.CommentSnippetId == snippetId);

            var comments = new List<Comment>();

            comments = ascQuery.Where(el => el.CommentCreatedDateTime >= queryPivot.PivotDateTime && el.CommentId >= queryPivot.PivotId)
                    .Take(size).Reverse().ToList();

            if (comments.Count < size)
            {
                var addToBottomSize = size - comments.Count;

                var descQuery = _applicationDbContext.Comments.Include(el => el.OwnerUser)
                    .OrderByDescending(el => el.CommentCreatedDateTime).ThenByDescending(el => el.CommentId)
                    .Where(el => el.Hidden == false && el.CommentSnippetId == snippetId);

                var bottomPartOfSnippets = await descQuery.Where(el => el.CommentCreatedDateTime <= queryPivot.PivotDateTime && el.CommentId < queryPivot.PivotId)
                    .Take(addToBottomSize).ToListAsync();

                comments.AddRange(bottomPartOfSnippets);
            }

            foreach (var comment in comments)
            {
                comment.CommentLikesCount = 0;
            }

            return comments;
        }

        public async Task<Comment> UpdateComment(Comment comment)
        {
            var found = await _applicationDbContext.Comments.FirstAsync(el => el.CommentId == comment.CommentId);
            found.CommentBody = comment.CommentBody;
            found.CommentLastUpdatedDateTime = comment.CommentLastUpdatedDateTime;

            await _applicationDbContext.SaveChangesAsync();
            return found;
        }

        public async Task<bool> IsExistNewerComment(QueryPivot queryPivot, Guid snippetId)
        {
            var descQuery = _applicationDbContext.Comments
                .OrderByDescending(el => el.CommentCreatedDateTime).ThenByDescending(el => el.CommentId).Where(el => el.Hidden == false && el.CommentSnippetId == snippetId);

            var newerCommentsCount = await descQuery.Where(
                el => el.CommentCreatedDateTime >= queryPivot.PivotDateTime && el.CommentId > queryPivot.PivotId)
                    .CountAsync();

            return newerCommentsCount != 0;
        }

        public async Task<bool> IsExistOlderComment(QueryPivot queryPivot, Guid snippetId)
        {
            var descQuery = _applicationDbContext.Comments
                .OrderByDescending(el => el.CommentCreatedDateTime).ThenByDescending(el => el.CommentId).Where(el => el.Hidden == false && el.CommentSnippetId == snippetId);

            var olderCommentsCount = await descQuery.Where(
                el => el.CommentCreatedDateTime <= queryPivot.PivotDateTime && el.CommentId < queryPivot.PivotId
                ).CountAsync();

            return olderCommentsCount != 0;
        }
    }
}
