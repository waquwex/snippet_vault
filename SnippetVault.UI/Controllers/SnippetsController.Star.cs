using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.StarDTOs;
using SnippetVault.Core.Helpers;

namespace SnippetVault.UI.Controllers
{
    public partial class SnippetsController
    {

        // Client side Action
        [Authorize]
        [Route("[action]/{snippetId}")]
        [HttpGet]
        public async Task<JsonResult> Star([FromRoute] Guid snippetId)
        {
            var currentUser = _userManager.GetUserGuid(User);

            var newActive = await _starService.StarSnippet(currentUser, snippetId);

            var json = new { Active = newActive };

            return new JsonResult(json);
        }

        // Client side Action
        [Authorize]
        [Route("[action]/{snippetId}")]
        [HttpGet]
        public async Task<JsonResult> SnippetStarCount([FromRoute] Guid snippetId)
        {
            // Get total, and users star active
            var starCount = await _snippetService.GetSnippetStarCountBySnippetId(snippetId);
            var json = new { StarCount = starCount };
            return new JsonResult(json);
        }

        // Client side Action
        [Authorize]
        [Route("[action]/{snippetId}")]
        [HttpGet]
        public async Task<JsonResult> GetStarActiveForUser([FromRoute] Guid snippetId)
        {
            var currentUserId = _userManager.GetUserGuid(User);
            var starResponse = await _starService.GetStarByOwnerIdAndSnippetId(currentUserId, snippetId);
            var json = new { Active = starResponse == null ? false : starResponse.StarActive };
            return new JsonResult(json);
        }


        [Route("[action]")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyStarredSnippets(long? pivotDateTimeTicks, Guid? pivotId, bool newer)
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

            List<StarResponse> starResponses;

            var currentUser = _userManager.GetUserGuid(User);

            if (newer)
            {
                starResponses = (await _snippetService.GetNewerStarredSnippets(pivot, 5, currentUser, 10)).ToList();
            }
            else
            {
                starResponses = (await _snippetService.GetOlderStarredSnippets(pivot, 5, currentUser, 10)).ToList();
            }

            ViewBag.IsItFirstPage = true;
            ViewBag.IsItLastPage = true;
            if (starResponses.Count > 0)
            {
                ViewBag.IsItFirstPage = !(await _snippetService.IsExistNewerStarredSnippet(
                    new QueryPivot(starResponses[0].LastUpdateTime.Value, starResponses[0].StarId.Value), currentUser));
                ViewBag.IsItLastPage = !(await _snippetService.IsExistOlderStarredSnippet(
                    new QueryPivot(starResponses[starResponses.Count - 1].LastUpdateTime.Value,
                    starResponses[starResponses.Count - 1].StarId.Value), currentUser));
            }

            return View(starResponses);
        }
    }
}