using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.IdentityEntities;

namespace SnippetVault.Core.DTO.SnippetDTOs
{
    public class SnippetResponse
    {
        public Guid? SnippetId { get; set; }

        public Guid? SnippetOwnerUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public string? SnippetTitle { get; set; }

        public string? SnippetDescription { get; set; }

        public string? SnippetFileName { get; set; }

        public string? SnippetBody { get; set; }

        public int SnippetStars { get; set; }
        
        public int SnippetCommentCount { get; set; }

        public DateTime? SnippetCreatedDateTime { get; set; }

        public DateTime? SnippetLastUpdateDateTime { get; set; }

        // This should get all comments of snippet
        public ICollection<Comment>? SnippetComments { get; set; }

        public bool CurrentUserStarStatus { get; set; }

        public SnippetUpdateRequest ToSnippetUpdateRequest()
        {
            return new SnippetUpdateRequest()
            {
                SnippetId = this.SnippetId,
                SnippetTitle = this.SnippetTitle,
                SnippetDescription = this.SnippetDescription,
                SnippetFileName = this.SnippetFileName,
                SnippetBody = this.SnippetBody
            };
        }
    }

    public static class SnippetExtensions
    {
        public static SnippetResponse ToSnippetResponse(this Snippet snippet)
        {
            return new SnippetResponse()
            {
                ApplicationUser = snippet.OwnerUser,
                SnippetCreatedDateTime = snippet.SnippetCreatedDateTime,
                SnippetLastUpdateDateTime = snippet.SnippetLastUpdateDateTime,
                SnippetBody = snippet.SnippetBody,
                SnippetComments = snippet.Comments,
                SnippetDescription = snippet.SnippetDescription,
                SnippetFileName = snippet.SnippetFileName,
                SnippetId = snippet.SnippetId,
                SnippetOwnerUserId = snippet.OwnerUserId,
                SnippetStars = snippet.SnippetStarsCount,
                SnippetTitle = snippet.SnippetTitle,
                SnippetCommentCount = snippet.SnippetCommentsCount
            };
        }
    }
}
