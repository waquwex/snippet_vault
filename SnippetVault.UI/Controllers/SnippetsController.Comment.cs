using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.UI.Filters.ActionFilters;

namespace SnippetVault.UI.Controllers
{
    public partial class SnippetsController
    {

        [Authorize]
        [Route("[action]/{snippetId}")]
        [HttpGet]
        public async Task<IActionResult> CreateComment([FromRoute] Guid snippetId)
        {
            var commentAddRequest = new CommentAddRequest();
            commentAddRequest.CommentSnippetId = snippetId;

            var snippetResponse = await _snippetService.GetSnippetById(snippetId);

            ViewBag.SnippetResponse = snippetResponse;

            return View(commentAddRequest);
        }

        [Authorize]
        [Route("[action]/{snippetId}")]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromForm] CommentAddRequest commentAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(commentAddRequest);
            }

            commentAddRequest.CommentOwnerUserId = _userManager.GetUserGuid(User);

            await _commentService.AddComment(commentAddRequest);

            return LocalRedirect("/snippets/" + commentAddRequest.CommentSnippetId);
        }

        [Authorize]
        [TypeFilter(typeof(AuthorizeCommentOwner))]
        [Route("[action]/{commentId}")]
        [HttpGet]
        public async Task<IActionResult> EditComment([FromRoute] Guid commentId)
        {
            var comment = await _commentService.GetCommentById(commentId);
            var commentUpdateRequest = comment.ToCommentUpdateRequest();

            var snippetResponse = await _snippetService.GetSnippetById(comment.CommentSnippetId.Value);
            ViewBag.SnippetResponse = snippetResponse;

            return View(commentUpdateRequest);
        }

        [Authorize]
        [TypeFilter(typeof(AuthorizeCommentOwner))]
        [Route("[action]/{snippetId}")]
        [HttpPost]
        public async Task<IActionResult> EditComment([FromForm] CommentUpdateRequest commentUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                var snippetResponse = await _snippetService.GetSnippetById(commentUpdateRequest.CommentSnippetId.Value);
                ViewBag.SnippetResponse = snippetResponse;

                return View(commentUpdateRequest);
            }

            await _commentService.UpdateComment(commentUpdateRequest);

            return LocalRedirect("/snippets/" + commentUpdateRequest.CommentSnippetId);
        }

        [Authorize]
        [TypeFilter(typeof(AuthorizeCommentOwner))]
        [Route("[action]/{commentId}")]
        [HttpGet]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            var commentResponse = await _commentService.GetCommentById(commentId);

            await _commentService.DeleteCommentById(commentId);

            return LocalRedirect("/snippets/" + commentResponse.CommentSnippetId);
        }

        // Client-side action
        [Authorize]
        [Route("[action]/{commentId}")]
        [HttpGet]
        public async Task<JsonResult> LikeComment([FromRoute] Guid commentId)
        {
            var userId = _userManager.GetUserGuid(User);

            var newLikeStatus = await _commentLikeService.LikeComment(userId, commentId);

            var json = new { NewLikeStatus = newLikeStatus };
            return new JsonResult(json);
        }

        // Client-side action
        [Authorize]
        [Route("[action]/{commentId}")]
        [HttpGet]
        public async Task<JsonResult> DislikeComment([FromRoute] Guid commentId)
        {
            var userId = _userManager.GetUserGuid(User);

            var newLikeStatus = await _commentLikeService.DislikeComment(userId, commentId);

            var json = new { NewLikeStatus = newLikeStatus };
            return new JsonResult(json);
        }
    }
}
