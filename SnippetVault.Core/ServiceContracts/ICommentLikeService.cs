using SnippetVault.Core.DTO.CommentDTOs;

namespace SnippetVault.Core.ServiceContracts
{
    public interface ICommentLikeService
    {
        /// <summary>
        /// Adds new comment to repository
        /// </summary>
        /// <param name="commentLikeAddRequest"></param>
        /// <returns></returns>
        Task<CommentLikeResponse> AddCommentLike(CommentLikeAddRequest commentLikeAddRequest);

        /// <summary>
        /// If there is not "like" for specified user and comment, add comment with comment size 1
        /// If there is "like" with comment size -1 for specified user and comment, update comment with comment size 1
        /// If there is "like" with comment size 1 for specified user and comment, update comment with comment size 0
        /// </summary>
        /// <returns>Newly updated comment like size</returns>
        Task<sbyte> LikeComment(Guid ownerId, Guid commentId);

        /// <summary>
        /// If there is not "like" for specified user and comment, add comment with comment size -1
        /// If there is "like" with comment size 1 for specified user and comment, update comment with comment size -1
        /// If there is "like" with comment size -1 for specified user and comment, update comment with comment size 0
        /// </summary>
        /// <returns>Newly updated comment like size<</returns>
        Task<sbyte> DislikeComment(Guid ownerId, Guid commentId);

        /// <summary>
        /// Delete comment with matching id
        /// </summary>
        /// <param name="commentLikeId"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentLikeById(Guid commentLikeId);


        /// <summary>
        /// Get commentlike by owner id and comment id
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<CommentLikeResponse?> GetCommentLikeByOwnerUserIdAndCommentId(Guid ownerId, Guid snippetId);
    }
}
