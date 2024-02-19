using SnippetVault.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Domain.RepositoryContracts
{
    public interface ICommentLikeRepository
    {
        /// <summary>
        /// Adds comment like to data store
        /// </summary>
        /// <param name="commentLike"></param>
        /// <returns></returns>
        Task<CommentLike> AddCommentLike(CommentLike commentLike);

        /// <summary>
        /// Gets comment with matching id
        /// </summary>
        /// <param name="commentLikeId"></param>
        /// <returns></returns>
        Task<CommentLike> GetCommentLikeById(Guid commentLikeId);

        /// <summary>
        /// Delete comment with matching id
        /// </summary>
        /// <param name="starId"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentLikeById(Guid starId);

        /// <summary>
        /// Update comment with matching id
        /// </summary>
        /// <param name="commentLike"></param>
        /// <returns></returns>
        Task<CommentLike> UpdateCommentLike(CommentLike commentLike);

        /// <summary>
        /// Gets comment like if owner user id and comment id matches with a comment in data store
        /// </summary>
        /// <param name="ownerUserId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<CommentLike?> GetCommentLikeByOwnerUserIdAndCommentId(Guid ownerUserId, Guid commentId);
    }
}