using SnippetVault.Core.Domain.Entities;

namespace SnippetVault.Core.Domain.IdentityEntities
{
    public class ApplicationUser: BaseUser
    {
        public string? AvatarImage { get; set; }

        public virtual ICollection<Snippet>? Snippets { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        
        public virtual ICollection<Star>? Stars { get; set; }
        public virtual ICollection<CommentLike>? CommentLikes { get; set; }

        public DateTime LastSentConfirmEmailTime { get; set; } = new DateTime();
    }
}