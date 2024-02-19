using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.DTO.StarDTOs;
using SnippetVault.Core.Exceptions;
using SnippetVault.Core.Helpers;
using SnippetVault.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVault.Core.Services
{
    public class StarService : IStarService
    {
        private readonly IStarRepository _starRepository;
        private readonly ISnippetRepository _snippetRepository;

        public StarService(IStarRepository starRepository, ISnippetRepository snippetRepository)
        {
            _starRepository = starRepository;
            _snippetRepository = snippetRepository;
        }

        public async Task<StarResponse> AddStar(StarAddRequest starAddRequest)
        {
            Utils.ValidateModel(starAddRequest);

            var star = starAddRequest.ToStar();
            star.StarActive = true;

            var addedStar = await _starRepository.AddStar(star);

            return addedStar.ToStarResponse();
        }

        public async Task<bool> DeleteStarById(Guid starId)
        {
            return await _starRepository.DeleteStarById(starId);
        }

        public async Task<StarResponse?> GetStarByOwnerIdAndSnippetId(Guid ownerId, Guid snippetId)
        {
            var star = await _starRepository.GetStarByOwnerIdAndSnippetId(ownerId, snippetId);
            if (star == null)
            {
                return null;
            }

            return star.ToStarResponse();
        }

        // This logic might fits in controller responsibility not service responsibility
        public async Task<bool> StarSnippet(Guid ownerId, Guid snippetId)
        {
            var snippet = await _snippetRepository.GetSnippetById(snippetId);
            if (snippet.Hidden == true)
            {
                throw new SnippetIsHiddenException();
            }

            var found = await _starRepository.GetStarByOwnerIdAndSnippetId(ownerId, snippetId);

            if (found != null)
            {
                var newActive = !found.StarActive;
                found.StarActive = newActive;
                await _starRepository.UpdateStar(found);
                return newActive;
            }
            else
            {
                var starAddRequest = new StarAddRequest()
                {
                    OwnerUserId = ownerId,
                    StarredSnippetId = snippetId,
                };

                await this.AddStar(starAddRequest);
                return true;
            }
        }
    }
}