using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.Helpers;
using SnippetVault.Core.ServiceContracts;


namespace SnippetVault.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResponse> AddComment(CommentAddRequest commentAddRequest)
        {
            Utils.ValidateModel(commentAddRequest);

            var comment = commentAddRequest.ToComment();
            comment.CommentCreatedDateTime = DateTime.UtcNow;
            comment.CommentLastUpdatedDateTime = DateTime.Now;

            var addedComment = await _commentRepository.AddComment(comment);

            return addedComment.ToCommentResponse();
        }

        public async Task<bool> DeleteCommentById(Guid id)
        {
            var success = await _commentRepository.DeleteCommentById(id);
            return success;
        }

        public async Task<CommentResponse> GetCommentById(Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);
            return comment.ToCommentResponse();
        }

        public async Task<ICollection<CommentResponse>> GetOlderComments(QueryPivot? queryPivot, int size, Guid snippetId)
        {
            var comments = await _commentRepository.GetOlderComments(queryPivot, size, snippetId);

            return comments.Select(el => el.ToCommentResponse()).ToList();
        }

        public async Task<ICollection<CommentResponse>> GetNewerComments(QueryPivot queryPivot, int size, Guid snippetId)
        {
            var comments = await _commentRepository.GetNewerComments(queryPivot, size, snippetId);

            return comments.Select(el => el.ToCommentResponse()).ToList();
        }

        public async Task<CommentResponse> UpdateComment(CommentUpdateRequest commentUpdateRequest)
        {
            Utils.ValidateModel(commentUpdateRequest);

            var comment = commentUpdateRequest.ToComment();
            comment.CommentLastUpdatedDateTime = DateTime.UtcNow;

            var updatedComment = await _commentRepository.UpdateComment(comment);
            return updatedComment.ToCommentResponse();
        }

        public Task<bool> IsExistNewerComment(QueryPivot pivot, Guid snippetId)
        {
            return _commentRepository.IsExistNewerComment(pivot, snippetId);
        }

        public Task<bool> IsExistOlderComment(QueryPivot pivot, Guid snippetId)
        {
            return _commentRepository.IsExistOlderComment(pivot, snippetId);
        }
    }
}