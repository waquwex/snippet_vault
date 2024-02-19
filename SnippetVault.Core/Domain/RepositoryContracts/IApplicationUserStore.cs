using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Domain.RepositoryContracts
{
    public interface IApplicationUserStore
    {
        Task WipeUserData(Guid userId);

        Task<int> GetSnippetsCount(Guid userId);

        Task<int> GetStarsCount(Guid userId);

        Task<int> GetCommentsCount(Guid userId);

        Task<int> GetCommentLikesCount(Guid userId);

        Task<DateTime> GetLastEmailConfirmSent(Guid userId);

        Task UpdateLastEmailConfirmSent(Guid userId);
    }
}