using SnippetVault.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SnippetVault.Core.DTO.CommentDTOs
{
    public class CommentLikeUpdateRequest
    {
        [Key]
        public Guid? CommentLikeId { get; set; }

        [Required, Range(-1, 1)]
        public sbyte CommentLikeSize { get; set; }

        public CommentLike ToCommentLike()
        {
            return new CommentLike()
            {
                CommentLikeId = this.CommentLikeId,
                CommentLikeSize = this.CommentLikeSize
            };
        }
    }
}