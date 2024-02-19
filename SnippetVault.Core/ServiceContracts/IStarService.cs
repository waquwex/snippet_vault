using SnippetVault.Core.DTO.StarDTOs;

namespace SnippetVault.Core.ServiceContracts
{
    public interface IStarService
    {
        /// <summary>
        /// Add star to repository
        /// </summary>
        /// <param name="starAddRequest"></param>
        /// <returns></returns>
        Task<StarResponse> AddStar(StarAddRequest starAddRequest);

        /// <summary>
        /// If there is no "star" for specified ownerId and snippetId, create a new star with StarActive equal to true
        /// If there is "star" for specified ownerId and snippetId and StarActive equal to false, update StarActive to true
        /// If there is "star" for specified ownerId and snippetId and StarActive equal to true, update StarActive to false
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        Task<bool> StarSnippet(Guid ownerId, Guid snippetId);

        /// <summary>
        /// Delete star with matching id
        /// </summary>
        /// <param name="starId"></param>
        /// <returns></returns>
        Task<bool> DeleteStarById(Guid starId);

        /// <summary>
        /// Gets star by snippet id and owner id
        /// </summary>
        /// <param name="snippetId"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<StarResponse?> GetStarByOwnerIdAndSnippetId(Guid ownerId, Guid snippetId);
    }
}