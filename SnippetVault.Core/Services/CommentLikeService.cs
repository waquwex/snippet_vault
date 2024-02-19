using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.ServiceContracts;
using SnippetVault.Core.Helpers;
using SnippetVault.Core.Exceptions;

namespace SnippetVault.Core.Services
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentLikeService(ICommentLikeRepository commentLikeRepository, ICommentRepository commentRepository)
        {
            _commentLikeRepository = commentLikeRepository;
            _commentRepository = commentRepository;
        }

        public async Task<CommentLikeResponse> AddCommentLike(CommentLikeAddRequest commentLikeAddRequest)
        {
            Utils.ValidateModel(commentLikeAddRequest);

            var commentLike = commentLikeAddRequest.ToCommentLike();
            var addedCommentLike = await _commentLikeRepository.AddCommentLike(commentLike);
            
            return addedCommentLike.ToCommentLikeResponse();
        }

        public async Task<bool> DeleteCommentLikeById(Guid commentLikeId)
        {
            var success = await _commentLikeRepository.DeleteCommentLikeById(commentLikeId);
            return success;
        }

        // This logic might fits in controller responsibility not service responsibility
        public async Task<sbyte> DislikeComment(Guid ownerId, Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);
            if (comment.Hidden == true)
            {
                throw new CommentIsHiddenException();
            }

            var found = await _commentLikeRepository.GetCommentLikeByOwnerUserIdAndCommentId(ownerId, commentId);
            if (found != null)
            {
                if (found.CommentLikeSize == -1)
                {
                    found.CommentLikeSize = 0;
                }
                else if (found.CommentLikeSize == 0 || found.CommentLikeSize == 1)
                {
                    found.CommentLikeSize = -1;
                }

                var updatedCommentLike = await _commentLikeRepository.UpdateCommentLike(found);
                return updatedCommentLike.CommentLikeSize;
            }
            else
            {
                var commentLikeAddRequest = new CommentLikeAddRequest()
                {
                    CommentLikeOwnerId = ownerId,
                    CommentId = commentId,
                    CommentLikeSize = -1
                };

                var addedCommentLike = await this.AddCommentLike(commentLikeAddRequest);
                return addedCommentLike.CommentLikeSize;
            }
        }

        public async Task<CommentLikeResponse?> GetCommentLikeByOwnerUserIdAndCommentId(Guid ownerId, Guid snippetId)
        {
            var commentLike = await _commentLikeRepository.GetCommentLikeByOwnerUserIdAndCommentId(ownerId, snippetId);

            if (commentLike == null)
            {
                return null;
            }

            return commentLike.ToCommentLikeResponse();
        }

        // This logic might fits in controller responsibility not service responsibility
        public async Task<sbyte> LikeComment(Guid ownerId, Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(commentId);
            if (comment.Hidden == true)
            {
                throw new CommentIsHiddenException();
            }

            var found = await _commentLikeRepository.GetCommentLikeByOwnerUserIdAndCommentId(ownerId, commentId);
            if (found != null)
            {
                if (found.CommentLikeSize == -1 || found.CommentLikeSize == 0)
                {
                    found.CommentLikeSize = 1;
                }
                else if (found.CommentLikeSize == 1)
                {
                    found.CommentLikeSize = 0;
                }

                var updatedCommentLike = await _commentLikeRepository.UpdateCommentLike(found);
                return updatedCommentLike.CommentLikeSize;
            }
            else
            {
                var commentLikeAddRequest = new CommentLikeAddRequest()
                {
                    CommentLikeOwnerId = ownerId,
                    CommentId = commentId,
                    CommentLikeSize = 1
                };

                var addedCommentLike = await this.AddCommentLike(commentLikeAddRequest);
                return addedCommentLike.CommentLikeSize;
            }
        }
    }
}
