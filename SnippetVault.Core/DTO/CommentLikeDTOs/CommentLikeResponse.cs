using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace SnippetVault.Core.DTO.CommentDTOs
{
    public class CommentLikeResponse
    {
        [Key]
        public Guid? CommentLikeId { get; set; }

        [Required]
        public Guid? CommentLikeOwnerId { get; set; }

        [Required]
        public Guid? CommentId { get; set; }

        [Required, Range(-1, 1)]
        public sbyte CommentLikeSize { get; set; }

        public CommentLikeUpdateRequest ToCommentLikeUpdateRequest()
        {
            return new CommentLikeUpdateRequest()
            {
                CommentLikeId = this.CommentLikeId,
                CommentLikeSize = this.CommentLikeSize
            };
        }
    }

    public static class CommentLikeExtensions
    {
        public static CommentLikeResponse ToCommentLikeResponse(this CommentLike commentLike)
        {
            return new CommentLikeResponse()
            {
                CommentLikeId = commentLike.CommentLikeId,
                CommentLikeOwnerId = commentLike.OwnerUserId,
                CommentId = commentLike.CommentId,
                CommentLikeSize = commentLike.CommentLikeSize
            };
        }
    }
}
