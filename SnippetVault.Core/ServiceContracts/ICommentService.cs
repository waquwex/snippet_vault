using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.Helpers;

namespace SnippetVault.Core.ServiceContracts
{
    public interface ICommentService
    {
        /// <summary>
        /// Adds comment to repository if passes validation
        /// </summary>
        /// <param name="commentAddRequest"></param>
        /// <returns></returns>
        Task<CommentResponse> AddComment(CommentAddRequest commentAddRequest);

        /// <summary>
        /// Get comment with matching id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<CommentResponse> GetCommentById(Guid commentId);

        /// <summary>
        /// Get older comments with pivoting a entry
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <param name="ownerId"></param>
        /// <param name="trimToLines"></param>
        /// <returns></returns>
        Task<ICollection<CommentResponse>> GetOlderComments(QueryPivot? queryPivot, int size, Guid snippetId);

        /// <summary>
        /// Get newer comments with pivoting a entry
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<ICollection<CommentResponse>> GetNewerComments(QueryPivot queryPivot, int size, Guid snippetId);

        /// <summary>
        /// Update comment with matching id, if passes validation
        /// </summary>
        /// <param name="commentUpdateRequest"></param>
        /// <returns></returns>
        Task<CommentResponse> UpdateComment(CommentUpdateRequest commentUpdateRequest);

        /// <summary>
        /// Delete comment with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentById(Guid id);

        /// <summary>
        /// Is there exist newer snippet from specified pivot entry
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> IsExistNewerComment(QueryPivot pivot, Guid snippetId);

        /// <summary>
        /// Is there exist older snippet from specified pivot entry
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> IsExistOlderComment(QueryPivot pivot, Guid snippetId);
    }
}