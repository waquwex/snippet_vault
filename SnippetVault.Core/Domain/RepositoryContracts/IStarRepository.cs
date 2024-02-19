using SnippetVault.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Domain.RepositoryContracts
{
    public interface IStarRepository
    {
        /// <summary>
        /// Adds star to data store
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        Task<Star> AddStar(Star star);

        /// <summary>
        /// Gets star with matching id
        /// </summary>
        /// <param name="starId"></param>
        /// <returns>if found matching star if not null</returns>
        Task<Star> GetStarById(Guid starId);

        /// <summary>
        /// Deletes star with matching star id from data store
        /// </summary>
        /// <param name="starId"></param>
        /// <returns></returns>
        Task<bool> DeleteStarById(Guid starId);

        /// <summary>
        /// Updates star member values with matching id
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        Task<Star> UpdateStar(Star star);

        /// <summary>
        /// Gets star with matching owner user id and snippet id
        /// </summary>
        /// <returns></returns>
        Task<Star?> GetStarByOwnerIdAndSnippetId(Guid ownerUserId, Guid snippetId);
    }
}