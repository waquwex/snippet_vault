using System.ComponentModel.DataAnnotations;

namespace SnippetVault.Core.DTO.StarDTOs
{
    public class StarUpdateRequest
    {
        [Required]
        public bool StarActive { get; set; }
    }
}