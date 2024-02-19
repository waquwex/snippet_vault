using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.DTO.SnippetDTOs;

namespace SnippetVault.UI.ViewModels
{
    public class SnippetDetailsViewModel
    {
        public SnippetResponse? SnippetResponse{ get; set; }
        public bool Owner { get; set; }

        public List<CommentResponse>? SnippetComments { get; set; }
    }
}
