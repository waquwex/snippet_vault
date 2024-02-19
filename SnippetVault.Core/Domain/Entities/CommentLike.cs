using SnippetVault.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnippetVault.Core.Domain.Entities
{
    public class CommentLike
    {
        [Key]
        public Guid? CommentLikeId { get; set; }

        public Guid? OwnerUserId { get; set; }

        public virtual ApplicationUser? OwnerUser { get; set; }

        [Required]
        public Guid? CommentId { get; set; }

        public virtual Comment? Comment { get; set; }

        [Required, Range(-1, 1)]
        public sbyte CommentLikeSize { get; set; }
    }
}