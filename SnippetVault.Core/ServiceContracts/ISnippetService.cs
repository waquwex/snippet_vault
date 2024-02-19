using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.DTO.StarDTOs;
using SnippetVault.Core.Helpers;
using System.Security.Claims;

namespace SnippetVault.Core.ServiceContracts
{
    public interface ISnippetService
    {
        /// <summary>
        /// Add snippet to repository
        /// </summary>
        /// <param name="snippetAddRequest"></param>
        /// <returns></returns>
        Task<SnippetResponse> AddSnippet(SnippetAddRequest snippetAddRequest);

        /// <summary>
        /// Get snippet with matching id
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<SnippetResponse> GetSnippetById(Guid snippetId);

        /// <summary>
        /// Get snippets with pivoting a entry
        /// </summary>
        /// <param name="cursorSnippetCreatedDatetime"></param>
        /// <param name="cursorSnippetId"></param>
        /// <param name="size"></param>
        /// <param name="paginationDirection"></param>
        /// <param name="expandToSize"></param>
        /// <returns></returns>
        Task<ICollection<SnippetResponse>> GetOlderSnippets(QueryPivot? queryPivot, int size, Guid? ownerId = null, int? trimToLines = null);

        /// <summary>
        /// Get snippets with pivoting a entry
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<ICollection<SnippetResponse>> GetNewerSnippets(QueryPivot queryPivot, int size, Guid? ownerId = null, int? trimToLines = null);

        /// <summary>
        /// Update snippet with matching id
        /// </summary>
        /// <param name="snippetUpdateRequest"></param>
        /// <returns></returns>
        Task<SnippetResponse> UpdateSnippet(SnippetUpdateRequest snippetUpdateRequest);

        /// <summary>
        /// Delete snippet with matching id
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> DeleteSnippetById(Guid snippetId);

        /// <summary>
        /// Updates hidden member value of snippet with matching id
        /// </summary>
        /// <param name="snippetId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        Task<bool> UpdateSnippetHiddenValueById(Guid snippetId, bool newValue);

        /// <summary>
        /// Is there newer Snippets from specified pivot
        /// </summary>
        /// <param name="pivot"></param>
        /// <returns></returns>
        Task<bool> IsExistNewerSnippet(QueryPivot pivot, Guid? ownerId = null);

        /// <summary>
        /// Is there older Snippets from specified pivot
        /// </summary>
        /// <param name="pivot"></param>
        /// <returns></returns>
        Task<bool> IsExistOlderSnippet(QueryPivot pivot, Guid? ownerId = null);

        /// <summary>
        /// Get starred snippets of user; with older created datetime
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <param name="ownerId"></param>
        /// <param name="trimToLines"></param>
        /// <returns></returns>
        Task<ICollection<StarResponse>> GetOlderStarredSnippets(QueryPivot? queryPivot, int size, Guid starredUserId, int? trimToLines = null);

        /// <summary>
        /// Get starred snippets of user; with newer created datetime
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <param name="ownerId"></param>
        /// <param name="trimToLines"></param>
        /// <returns></returns>
        Task<ICollection<StarResponse>> GetNewerStarredSnippets(QueryPivot queryPivot, int size, Guid starredUserId, int? trimToLines = null);

        /// <summary>
        /// Is there newer starred snippet of user
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<bool> IsExistNewerStarredSnippet(QueryPivot pivot, Guid starredUserId);

        /// <summary>
        /// Is there older starred snippet of user
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<bool> IsExistOlderStarredSnippet(QueryPivot pivot, Guid starredUserId);

        /// <summary>
        /// Gets total star count of a snippet
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<int> GetSnippetStarCountBySnippetId(Guid snippetId);
    }
}