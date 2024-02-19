using SnippetVault.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;


namespace SnippetVault.Core.DTO.CommentDTOs
{
    public class CommentUpdateRequest
    {
        public Guid? CommentId { get; set; }

        [Required]
        public Guid? CommentSnippetId { get; set; }

        [Required, StringLength(8192)]
        public string? CommentBody { get; set; }

        public Comment ToComment()
        {
            return new Comment()
            {
                CommentId = this.CommentId,
                CommentBody = this.CommentBody,
            };
        }
    }
}