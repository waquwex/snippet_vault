using SnippetVault.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnippetVault.Core.Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid? CommentId { get; set; }

        public Guid? OwnerUserId { get; set; }

        public virtual ApplicationUser? OwnerUser { get; set; }

        [Required]
        public Guid? CommentSnippetId { get; set; }

        public virtual Snippet? Snippet { get; set; }

        [Required, StringLength(8192)]
        public string? CommentBody { get; set; }

        [Required]
        public DateTime? CommentCreatedDateTime { get; set; }

        [Required]
        public DateTime? CommentLastUpdatedDateTime { get; set; }

        public bool Hidden { get; set; }

        [NotMapped]
        public int CommentLikesCount { get; set; }

        public virtual ICollection<CommentLike>? CommentLikes { get; set; }
    }
}