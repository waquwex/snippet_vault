using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.DTO.SnippetDTOs;
using SnippetVault.Core.DTO.StarDTOs;
using SnippetVault.Core.Helpers;
using SnippetVault.Core.ServiceContracts;
using System.Security.Claims;

namespace SnippetVault.Core.Services
{
    public class SnippetService : ISnippetService
    {
        private readonly ISnippetRepository _snippetRepository;
        private readonly IApplicationUserManager _userManager;

        public SnippetService(ISnippetRepository snippetRepository, IApplicationUserManager userManager)
        {
            _snippetRepository = snippetRepository;
            _userManager = userManager;
        }

        public async Task<SnippetResponse> AddSnippet(SnippetAddRequest snippetAddRequest)
        {
            Utils.ValidateModel(snippetAddRequest);

            var snippet = snippetAddRequest.ToSnippet();
            snippet.OwnerUserId = _userManager.GetUserGuid(snippetAddRequest.SnippetOwnerUser);
            snippet.SnippetCreatedDateTime = DateTime.UtcNow;
            snippet.SnippetLastUpdateDateTime = DateTime.UtcNow;

            var addedSnippet = await _snippetRepository.AddSnippet(snippet);

            return addedSnippet.ToSnippetResponse();
        }

        public async Task<bool> DeleteSnippetById(Guid snippetId)
        {
            var success = await _snippetRepository.DeleteSnippetById(snippetId);
            return success;
        }

        public async Task<SnippetResponse> GetSnippetById(Guid snippetId)
        {
            var snippet = await _snippetRepository.GetSnippetById(snippetId);
            return snippet.ToSnippetResponse();
        }

        public async Task<ICollection<SnippetResponse>> GetOlderSnippets(QueryPivot? queryPivot, int size, Guid? ownerId = null, int? trimToLines = null)
        {
            var snippets = await _snippetRepository.GetOlderSnippets(queryPivot, size, ownerId);

            if (trimToLines != null && trimToLines > 0)
            {
                foreach (var snippet in snippets)
                {
                    snippet.SnippetBody = Utils.TrimToLines(snippet.SnippetBody, 10);
                }
            }

            return snippets.Select(el => el.ToSnippetResponse()).ToList();
        }

        public async Task<ICollection<SnippetResponse>> GetNewerSnippets(QueryPivot queryPivot, int size, Guid? ownerId = null, int? trimToLines = null)
        {
            var snippets = await _snippetRepository.GetNewerSnippets(queryPivot, size, ownerId);

            if (trimToLines != null && trimToLines > 0)
            {
                foreach (var snippet in snippets)
                {
                    snippet.SnippetBody = Utils.TrimToLines(snippet.SnippetBody, 10);
                }
            }

            return snippets.Select(el => el.ToSnippetResponse()).ToList();
        }

        public async Task<SnippetResponse> UpdateSnippet(SnippetUpdateRequest snippetUpdateRequest)
        {
            Utils.ValidateModel(snippetUpdateRequest);

            if (snippetUpdateRequest.SnippetId == null)
            {
                throw new ArgumentException("Snippet Id is null");
            }

            var snippet = snippetUpdateRequest.ToSnippet();
            snippet.SnippetLastUpdateDateTime = DateTime.UtcNow;

            var updatedSnippet = await _snippetRepository.UpdateSnippet(snippet);
            return updatedSnippet.ToSnippetResponse();
        }

        public async Task<bool> UpdateSnippetHiddenValueById(Guid snippetId, bool newValue)
        {
            var found = await _snippetRepository.GetSnippetById(snippetId);
            found.Hidden = newValue;
            await _snippetRepository.UpdateSnippet(found);
            return newValue;
        }

        public async Task<ICollection<SnippetResponse>> GetSnippetsByOwner(ClaimsPrincipal user, int bodyLines = 0)
        {
            var userId = _userManager.GetUserGuid(user);
            var snippets = await _snippetRepository.GetSnippetsByOwnerId(userId, bodyLines);
            return snippets.Select(s => s.ToSnippetResponse()).ToList();
        }

        public async Task<bool> IsExistNewerSnippet(QueryPivot pivot, Guid? ownerId = null)
        {
            return await _snippetRepository.IsExistNewerSnippet(pivot, ownerId);
        }

        public async Task<bool> IsExistOlderSnippet(QueryPivot pivot, Guid? ownerId = null)
        {
            return await _snippetRepository.IsExistsOlderSnippet(pivot, ownerId);
        }

        public async Task<ICollection<StarResponse>> GetOlderStarredSnippets(QueryPivot? queryPivot, int size, Guid starredUserId, int? trimToLines = null)
        {
            var stars = await _snippetRepository.GetOlderStarredSnippets(queryPivot, size, starredUserId);

            var starResponses = new List<StarResponse>();

            foreach (var star in stars)
            {
                var starResponse = star.ToStarResponse();
                starResponse.SnippetResponse = star.Snippet.ToSnippetResponse();
                starResponses.Add(starResponse);

                if (trimToLines != null && trimToLines > 0)
                {
                    starResponse.SnippetResponse.SnippetBody = Utils.TrimToLines(starResponse.SnippetResponse.SnippetBody, 10);
                }
            }

            return starResponses;
        }

        public async Task<ICollection<StarResponse>> GetNewerStarredSnippets(QueryPivot queryPivot, int size, Guid starredUserId, int? trimToLines = null)
        {
            var stars = await _snippetRepository.GetNewerStarredSnippets(queryPivot, size, starredUserId);
            var starResponses = new List<StarResponse>();

            foreach (var star in stars)
            {
                var starResponse = star.ToStarResponse();
                starResponse.SnippetResponse = star.Snippet.ToSnippetResponse();
                starResponses.Add(starResponse);

                if (trimToLines != null && trimToLines > 0)
                {
                    starResponse.SnippetResponse.SnippetBody = Utils.TrimToLines(starResponse.SnippetResponse.SnippetBody, 10);
                }
            }

            return starResponses;
        }

        public async Task<bool> IsExistNewerStarredSnippet(QueryPivot pivot, Guid starredUserId)
        {
            return await _snippetRepository.IsExistNewerStarredSnippet(pivot, starredUserId);
        }

        public async Task<bool> IsExistOlderStarredSnippet(QueryPivot pivot, Guid starredUserId)
        {
            return await _snippetRepository.IsExistOlderStarredSnippet(pivot, starredUserId);
        }

        public async Task<int> GetSnippetStarCountBySnippetId(Guid snippetId)
        {
            return await _snippetRepository.GetSnippetStarsCountById(snippetId);
        }
    }
}