using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.IdentityEntities;


namespace SnippetVault.Core.DTO.CommentDTOs
{
    public class CommentResponse
    {
        public Guid? CommentId { get; set; }

        public Guid? CommentOwnerUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public Guid? CommentSnippetId { get; set; }

        public Snippet? Snippet { get; set; }

        public string? CommentBody { get; set; }

        public DateTime? CommentCreatedDateTime { get; set; }

        public DateTime? CommentLastUpdatedDateTime { get; set; }

        public bool Owner { get; set; }

        public bool? UserLiked { get; set; }

        public CommentUpdateRequest ToCommentUpdateRequest()
        {
            return new CommentUpdateRequest()
            {
                CommentId = this.CommentId,
                CommentSnippetId = this.CommentSnippetId,
                CommentBody = this.CommentBody
            };
        }
    }

    public static class CommentExtensions
    {
        public static CommentResponse ToCommentResponse(this Comment comment)
        {
            return new CommentResponse()
            {
                ApplicationUser = comment.OwnerUser,
                CommentBody = comment.CommentBody,
                CommentCreatedDateTime = comment.CommentCreatedDateTime,
                CommentId = comment.CommentId,
                CommentLastUpdatedDateTime = comment.CommentLastUpdatedDateTime,
                CommentOwnerUserId = comment.OwnerUserId,
                CommentSnippetId = comment.CommentSnippetId,
                Snippet = comment.Snippet
            };
        }
    }
}
