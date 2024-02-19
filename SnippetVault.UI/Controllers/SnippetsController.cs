using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SnippetVault.Core.DTO.CommentDTOs;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.DTO.StarDTOs;
using SnippetVault.Core.Helpers;
using SnippetVault.Core.ServiceContracts;
using SnippetVault.UI.Filters.ActionFilters;
using SnippetVault.UI.ViewModels;

namespace SnippetVault.UI.Controllers
{
    [Route("[controller]")]
    public partial class SnippetsController : Controller
    {
        readonly ISnippetService _snippetService;
        readonly IApplicationUserManager _userManager;
        readonly IStarService _starService;
        readonly ICommentService _commentService;
        readonly ICommentLikeService _commentLikeService;

        public SnippetsController(ISnippetService snippetService, IApplicationUserManager userManager,
            IStarService starService, ICommentService commentService, ICommentLikeService commentLikeService)
        {
            _snippetService = snippetService;
            _userManager = userManager;
            _starService = starService;
            _commentService = commentService;
            _commentLikeService = commentLikeService;
        }
    }
}