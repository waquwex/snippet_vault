using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.DTO.SnippetDTOs;

namespace SnippetVault.Core.DTO.StarDTOs
{
    public class StarResponse
    {
        public Guid? StarId { get; set; }

        public Guid? OwnerUserId { get; set; }

        public Guid? StarredSnippetId { get; set; }

        public bool StarActive { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public SnippetResponse? SnippetResponse { get; set; }

        public StarUpdateRequest ToStarUpdateRequest()
        {
            return new StarUpdateRequest()
            {
                StarActive = StarActive
            };
        }
    }

    public static class StarExtensions
    {
        public static StarResponse ToStarResponse(this Star star)
        {
            return new StarResponse()
            {
                StarId = star.StarId,
                OwnerUserId = star.OwnerUserId,
                StarActive = star.StarActive,
                StarredSnippetId = star.SnippetId,
                LastUpdateTime = star.LastUpdateTime
            };
        }
    }
}
