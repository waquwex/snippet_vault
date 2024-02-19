using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SnippetVault.Core.DTO.SnippetDTOs
{
    public class SnippetAddRequest
    {
        public ClaimsPrincipal? SnippetOwnerUser { get; set; }


        [Required, StringLength(512, MinimumLength = 5)]
        public string? SnippetTitle { get; set; }

        [StringLength(1024)]
        public string? SnippetDescription { get; set; }

        [Required, StringLength(256, MinimumLength = 5)]
        public string? SnippetFileName { get; set; }

        [Required, StringLength(32768)]
        public string? SnippetBody { get; set; }

        public Snippet ToSnippet()
        {
            return new Snippet()
            {
                SnippetTitle = this.SnippetTitle,
                SnippetDescription = this.SnippetDescription,
                SnippetFileName = this.SnippetFileName,
                SnippetBody = this.SnippetBody
            };
        }
    }
}
