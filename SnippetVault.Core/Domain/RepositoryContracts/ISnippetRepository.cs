using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.Helpers;


namespace SnippetVault.Core.Domain.RepositoryContracts
{
    public interface ISnippetRepository
    {
        /// <summary>
        /// Adds a new snippet to the data store
        /// </summary>
        /// <param name="snippet"></param>
        /// <returns>Newly added snippet to data store</returns>
        Task<Snippet> AddSnippet(Snippet snippet);

        /// <summary>
        /// Get older snippets with pivot from data store
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size">Entries in page</param>
        /// <returns></returns>
        Task<ICollection<Snippet>> GetOlderSnippets(QueryPivot? queryPivot, int size, Guid? ownerId = null);

        /// <summary>
        /// Is there newer snippets exists
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<bool> IsExistNewerSnippet(QueryPivot queryPivot, Guid? ownerId = null);

        /// <summary>
        /// Get newer snippets with pivot from data store
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<ICollection<Snippet>> GetNewerSnippets(QueryPivot queryPivot, int size, Guid? ownerId = null);

        /// <summary>
        /// Is there older snippets exists
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<bool> IsExistsOlderSnippet(QueryPivot queryPivot, Guid? ownerId = null);

        /// <summary>
        /// Get snippet with matching snippet id
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<Snippet> GetSnippetById(Guid snippetId);

        /// <summary>
        /// Updates the snippet member values with matching snippet id
        /// </summary>
        /// <param name="snippet">Newly updated snippet</param>
        /// <returns></returns>
        Task<Snippet> UpdateSnippet(Snippet snippet);

        /// <summary>
        /// Deletes snippet with matching snippet id
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> DeleteSnippetById(Guid snippetId);

        /// <summary>
        /// Gets star count of a snippet
        /// </summary>
        /// <param name="snippetById"></param>
        /// <returns></returns>
        Task<int> GetSnippetStarsCountById(Guid snippetId);


        /// <summary>
        /// Gets all snippets with matching owner id
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<ICollection<Snippet>> GetSnippetsByOwnerId(Guid ownerId, int bodyLines);


        /// <summary>
        /// Gets comment count of a snippet
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<int> GetSnippetCommentsCountById(Guid snippetId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <param name="ownerId"></param>
        /// <param name="trimToLines"></param>
        /// <returns></returns>
        public Task<ICollection<Star>> GetOlderStarredSnippets(QueryPivot? queryPivot, int size, Guid starredUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryPivot"></param>
        /// <param name="size"></param>
        /// <param name="ownerId"></param>
        /// <param name="trimToLines"></param>
        /// <returns></returns>
        public Task<ICollection<Star>> GetNewerStarredSnippets(QueryPivot queryPivot, int size, Guid starredUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public Task<bool> IsExistNewerStarredSnippet(QueryPivot pivot, Guid starredUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public Task<bool> IsExistOlderStarredSnippet(QueryPivot pivot, Guid starredUserId);
    }
}