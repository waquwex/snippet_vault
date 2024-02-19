using SnippetVault.Core.Domain.Entities;

namespace SnippetVault.Core.DTO.StarDTOs
{
    public class StarAddRequest
    {
        public Guid? OwnerUserId { get; set; }

        public Guid? StarredSnippetId { get; set; }

        public Star ToStar()
        {
            return new Star()
            {
                OwnerUserId = OwnerUserId,
                SnippetId = StarredSnippetId,
            };
        }
    }
}