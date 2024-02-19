using SnippetVault.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnippetVault.Core.Domain.Entities
{
    public class Snippet
    {
        [Key]
        public Guid? SnippetId { get; set; }

        public Guid? OwnerUserId { get; set; }

        public virtual ApplicationUser? OwnerUser { get; set; }

        [Required, StringLength(512, MinimumLength = 5)]
        public string? SnippetTitle { get; set; }

        [StringLength(1024)]
        public string? SnippetDescription { get; set; }

        [Required, StringLength(256, MinimumLength = 5)]
        public string? SnippetFileName { get; set; }

        [Required, StringLength(32768)]
        public string? SnippetBody { get; set; }

        [NotMapped]
        public int SnippetStarsCount { get; set; }

        [NotMapped]
        public int SnippetCommentsCount { get; set; }

        [Required]
        public DateTime? SnippetCreatedDateTime { get; set; }

        [Required]
        public DateTime? SnippetLastUpdateDateTime { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        public bool Hidden { get; set; }

        public virtual ICollection<Star>? Stars { get; set; }
    }
}