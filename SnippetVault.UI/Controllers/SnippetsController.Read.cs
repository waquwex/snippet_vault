using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.Helpers;
using SnippetVault.UI.ViewModels;

namespace SnippetVault.UI.Controllers
{
    public partial class SnippetsController
    {

        [Route("/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(long? pivotDateTimeTicks, Guid? pivotId, bool newer)
        {
            QueryPivot? pivot = null;

            if (pivotDateTimeTicks != null && pivotId != null)
            {
                pivot = new QueryPivot(new DateTime(pivotDateTimeTicks.Value), pivotId.Value);
            }
            else
            {
                if (newer)
                {
                    throw new Exception("If you want to user newer you should specifiy QueryPivot");
                }
            }

            List<SnippetResponse> snippetResponses;

            if (newer)
            {
                snippetResponses = (await _snippetService.GetNewerSnippets(pivot, 5, null, 6)).ToList();
            }
            else
            {
                snippetResponses = (await _snippetService.GetOlderSnippets(pivot, 5, null, 6)).ToList();
            }

            ViewBag.IsItFirstPage = true;
            ViewBag.IsItLastPage = true;
            if (snippetResponses.Count > 0)
            {
                ViewBag.IsItFirstPage = !(await _snippetService.IsExistNewerSnippet(
                    new QueryPivot(snippetResponses[0].SnippetCreatedDateTime.Value, snippetResponses[0].SnippetId.Value)));
                ViewBag.IsItLastPage = !(await _snippetService.IsExistOlderSnippet(
                    new QueryPivot(snippetResponses[snippetResponses.Count - 1].SnippetCreatedDateTime.Value,
                    snippetResponses[snippetResponses.Count - 1].SnippetId.Value)));

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var ownerId = _userManager.GetUserGuid(User);

                    foreach (var snippetResponse in snippetResponses)
                    {
                        var starResponse = await _starService.GetStarByOwnerIdAndSnippetId(ownerId, snippetResponse.SnippetId.Value);

                        if (starResponse == null)
                        {
                            snippetResponse.CurrentUserStarStatus = false;
                        }
                        else
                        {
                            snippetResponse.CurrentUserStarStatus = starResponse.StarActive;
                        }
                    }
                }
            }


            return View(snippetResponses);
        }

        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MySnippets(long? pivotDateTimeTicks, Guid? pivotId, bool newer)
        {
            QueryPivot? pivot = null;

            if (pivotDateTimeTicks != null && pivotId != null)
            {
                pivot = new QueryPivot(new DateTime(pivotDateTimeTicks.Value), pivotId.Value);
            }
            else
            {
                if (newer)
                {
                    throw new Exception("If you want to user newer you should specifiy QueryPivot");
                }
            }

            List<SnippetResponse> snippetResponses;

            var currentUser = _userManager.GetUserGuid(User);

            if (newer)
            {
                snippetResponses = (await _snippetService.GetNewerSnippets(pivot, 5, currentUser, 6)).ToList();
            }
            else
            {
                snippetResponses = (await _snippetService.GetOlderSnippets(pivot, 5, currentUser, 6)).ToList();
            }

            ViewBag.IsItFirstPage = true;
            ViewBag.IsItLastPage = true;
            if (snippetResponses.Count > 0)
            {
                ViewBag.IsItFirstPage = !(await _snippetService.IsExistNewerSnippet(
                    new QueryPivot(snippetResponses[0].SnippetCreatedDateTime.Value, snippetResponses[0].SnippetId.Value), currentUser));
                ViewBag.IsItLastPage = !(await _snippetService.IsExistOlderSnippet(
                    new QueryPivot(snippetResponses[snippetResponses.Count - 1].SnippetCreatedDateTime.Value,
                    snippetResponses[snippetResponses.Count - 1].SnippetId.Value), currentUser));

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var ownerId = _userManager.GetUserGuid(User);

                    foreach (var snippetResponse in snippetResponses)
                    {
                        var starResponse = await _starService.GetStarByOwnerIdAndSnippetId(ownerId, snippetResponse.SnippetId.Value);

                        if (starResponse == null)
                        {
                            snippetResponse.CurrentUserStarStatus = false;
                        }
                        else
                        {
                            snippetResponse.CurrentUserStarStatus = starResponse.StarActive;
                        }
                    }
                }
            }

            return View(snippetResponses);
        }

        [AllowAnonymous]
        [Route("{snippetId}")]
        public async Task<IActionResult> Details([FromRoute] Guid snippetId, long? commentPivotDateTimeTicks, Guid? commentPivotId, bool newer)
        {
            var vm = new SnippetDetailsViewModel();

            var snippetResponse = await _snippetService.GetSnippetById(snippetId);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var ownerId = _userManager.GetUserGuid(User);

                if (snippetResponse.SnippetOwnerUserId == ownerId)
                {
                    vm.Owner = true;
                }

                var starResponse = await _starService.GetStarByOwnerIdAndSnippetId(ownerId, snippetId);

                if (starResponse == null)
                {
                    snippetResponse.CurrentUserStarStatus = false;
                }
                else
                {
                    snippetResponse.CurrentUserStarStatus = starResponse.StarActive;
                }
            }

            vm.SnippetResponse = snippetResponse;

            QueryPivot? queryPivot = null;

            if (commentPivotDateTimeTicks != null && commentPivotId != null)
            {
                queryPivot = new QueryPivot(new DateTime(commentPivotDateTimeTicks.Value), commentPivotId.Value);
            }

            List<CommentResponse> snippetCommentResponses;

            if (newer)
            {
                snippetCommentResponses = (await _commentService.GetNewerComments(queryPivot, 5, snippetResponse.SnippetId.Value)).ToList();
            }
            else
            {
                snippetCommentResponses = (await _commentService.GetOlderComments(queryPivot, 5, snippetResponse.SnippetId.Value)).ToList();
            }

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                foreach (var snippetCommentResponse in snippetCommentResponses)
                {
                    var ownerId = _userManager.GetUserGuid(User);

                    var commentLikeResponse = await _commentLikeService.GetCommentLikeByOwnerUserIdAndCommentId(ownerId, snippetCommentResponse.CommentId.Value);

                    if (commentLikeResponse != null)
                    {
                        if (commentLikeResponse.CommentLikeSize == 1)
                        {
                            snippetCommentResponse.UserLiked = true;
                        }
                        else if (commentLikeResponse.CommentLikeSize == -1)
                        {
                            snippetCommentResponse.UserLiked = false;
                        }
                    }

                    snippetCommentResponse.Owner = snippetCommentResponse.CommentOwnerUserId == _userManager.GetUserGuid(User);
                }
            }


            vm.SnippetComments = snippetCommentResponses;

            ViewBag.IsItFirstPage = true;
            ViewBag.IsItLastPage = true;

            if (snippetCommentResponses.Count > 0)
            {
                ViewBag.IsItFirstPage = !(await _commentService.IsExistNewerComment(
                    new QueryPivot(snippetCommentResponses[0].CommentCreatedDateTime.Value,
                    snippetCommentResponses[0].CommentId.Value), snippetResponse.SnippetId.Value
                    ));
                ViewBag.IsItLastPage = !(await _commentService.IsExistOlderComment(
                    new QueryPivot(snippetCommentResponses[snippetCommentResponses.Count - 1].CommentCreatedDateTime.Value,
                    snippetCommentResponses[snippetCommentResponses.Count - 1].CommentId.Value), snippetResponse.SnippetId.Value
                    ));
            }

            ViewBag.CurrentUrl = Request.GetEncodedPathAndQuery();

            return View(vm);
        }
    }
}