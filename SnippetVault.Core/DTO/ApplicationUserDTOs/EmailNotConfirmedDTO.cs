using System.ComponentModel.DataAnnotations;

namespace SnippetVault.Core.DTO.ApplicationUserDTOs
{
    public class EmailNotConfirmedDTO
    {
        [Required()]
        [EmailAddress]
        public string? Email { get; set; }

        public bool? Success { get; set; }

        public bool? HaveSessionFlexName { get; set; }
    }
}