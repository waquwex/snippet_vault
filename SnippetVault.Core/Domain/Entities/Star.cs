using SnippetVault.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SnippetVault.Core.Domain.Entities
{
    public class Star
    {
        [Key]
        public Guid? StarId { get; set; }

        public Guid? OwnerUserId { get; set; }

        public virtual ApplicationUser? OwnerUser { get; set; }

        [Required]
        public Guid? SnippetId { get; set; }

        public virtual Snippet? Snippet { get; set; }

        public bool StarActive { get; set; } = true;

        public DateTime? LastUpdateTime {get; set;}
    }
}