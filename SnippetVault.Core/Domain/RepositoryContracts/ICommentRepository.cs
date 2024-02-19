using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Helpers;
using System.Collections;

namespace SnippetVault.Core.Domain.RepositoryContracts
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Adds a new comment to data store
        /// </summary>
        /// <param name="comment">Comment to add</param>
        /// <returns></returns>
        public Task<Comment> AddComment(Comment comment);


        /// <summary>
        /// Get older comments with pivot from data store
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ICollection<Comment>> GetOlderComments(QueryPivot? queryPivot, int size, Guid snippetId);


        /// <summary>
        /// Get newer comments with pivot from data store
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ICollection<Comment>> GetNewerComments(QueryPivot queryPivot, int size, Guid snippetId);


        /// <summary>
        /// Gets a comment with matching comment id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public Task<Comment> GetCommentById(Guid commentId);

        /// <summary>
        /// Updates the comment with matching comment id
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Updated comment</returns>
        public Task<Comment> UpdateComment(Comment comment);

        /// <summary>
        /// Deletes comment with matching comment id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public Task<bool> DeleteCommentById(Guid commentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> IsExistNewerComment(QueryPivot queryPivot, Guid snippetId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> IsExistOlderComment(QueryPivot queryPivot, Guid snippetId);
    }
}