using SnippetVault.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SnippetVault.Core.DTO.CommentDTOs
{
    public class CommentLikeAddRequest
    {
        [Required]
        public Guid? CommentLikeOwnerId { get; set; }

        [Required]
        public Guid? CommentId { get; set; }

        [Required, Range(-1, 1)]
        public sbyte CommentLikeSize { get; set; }


        public CommentLike ToCommentLike()
        {
            return new CommentLike()
            {
                OwnerUserId = this.CommentLikeOwnerId,
                CommentId = this.CommentId,
                CommentLikeSize = this.CommentLikeSize
            };
        }
    }
}