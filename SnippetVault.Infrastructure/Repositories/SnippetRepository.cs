using Microsoft.EntityFrameworkCore;
using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.Helpers;
using SnippetVault.Infrastructure.DbContext;
using System.Drawing;
using System.Security.Policy;


namespace SnippetVault.Infrastructure.Repositories
{
    public class SnippetRepository : ISnippetRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SnippetRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Snippet> AddSnippet(Snippet snippet)
        {
            await _applicationDbContext.Snippets.AddAsync(snippet);
            await _applicationDbContext.SaveChangesAsync();
            return snippet;
        }

        public async Task<bool> DeleteSnippetById(Guid snippetId)
        {
            _applicationDbContext.Snippets.RemoveRange(_applicationDbContext.Snippets.Where(el => el.SnippetId == snippetId));
            var rowsDeleted = await _applicationDbContext.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<Snippet> GetSnippetById(Guid snippetId)
        {
            var snippet = await _applicationDbContext.Snippets.Include(el => el.OwnerUser).Include(el => el.Comments)
                .FirstAsync(el => el.SnippetId == snippetId);
            
            snippet.SnippetStarsCount = await GetSnippetStarsCountById(snippetId);
            snippet.SnippetCommentsCount = snippet.Comments == null ? 0 : snippet.Comments.Count;
            return snippet;
        }

        // Returned pivot is last element or returned element
        // Default behavior is expand to size
        // Need to write Stored Procedure because it needs cursor for effectiveness
        public async Task<ICollection<Snippet>> GetOlderSnippets(QueryPivot? queryPivot, int size, Guid? ownerId = null)
        {
            var descQuery = _applicationDbContext.Snippets.Include(el => el.OwnerUser)
                    .OrderByDescending(el => el.SnippetCreatedDateTime).ThenByDescending(el => el.SnippetId).Where(el => el.Hidden == false);

            if (ownerId != null)
            {
                descQuery = descQuery.Where(el => el.OwnerUserId == ownerId);
            }

            var snippets = new List<Snippet>();

            if (queryPivot == null)
            {
                snippets = await descQuery.Take(size).ToListAsync();
            }
            else
            {
                snippets = await descQuery.Where(el => el.SnippetCreatedDateTime <= queryPivot.PivotDateTime && el.SnippetId < queryPivot.PivotId).Take(size).ToListAsync();
                
                if (snippets.Count < size)
                {
                    var addToTopSize = size - snippets.Count;

                    var ascQuery = _applicationDbContext.Snippets.Include(el => el.OwnerUser)
                            .OrderBy(el => el.SnippetCreatedDateTime).ThenBy(el => el.SnippetId).Where(el => el.Hidden == false);

                    var topPartOfSnippets = await ascQuery.Where(el => el.SnippetCreatedDateTime >= queryPivot.PivotDateTime && el.SnippetId >= queryPivot.PivotId)
                        .Take(addToTopSize).Reverse().ToListAsync();

                    topPartOfSnippets.AddRange(snippets);
                    snippets = topPartOfSnippets;
                }
            }

            foreach (var snippet in snippets)
            {
                var starsCount =  await GetSnippetStarsCountById(snippet.SnippetId.Value);
                var commentsCount = await GetSnippetCommentsCountById(snippet.SnippetId.Value);
                snippet.SnippetStarsCount = starsCount;
                snippet.SnippetCommentsCount = commentsCount;
            }

            return snippets;
        }

        public async Task<int> GetSnippetStarsCountById(Guid snippetId)
        {
            return await _applicationDbContext.Stars.Where(
                el => el.SnippetId == snippetId && el.StarActive == true).CountAsync();
        }

        public async Task<Snippet> UpdateSnippet(Snippet snippet)
        {
            var found = await _applicationDbContext.Snippets.FirstAsync(el => el.SnippetId == snippet.SnippetId);

            found.SnippetBody = snippet.SnippetBody;
            found.SnippetDescription = snippet.SnippetDescription;
            found.SnippetFileName = snippet.SnippetFileName;
            found.SnippetLastUpdateDateTime = DateTime.UtcNow;
            found.SnippetTitle = snippet.SnippetTitle;

            await _applicationDbContext.SaveChangesAsync();
            
            return found;
        }

        public async Task<ICollection<Snippet>> GetNewerSnippets(QueryPivot queryPivot, int size, Guid? ownerId = null)
        {
            var ascQuery = _applicationDbContext.Snippets.Include(el => el.OwnerUser)
                .OrderBy(el => el.SnippetCreatedDateTime).ThenBy(el => el.SnippetId).Where(el => el.Hidden == false);

            if (ownerId != null)
            {
                ascQuery = ascQuery.Where(el => el.OwnerUserId == ownerId);
            }

            var snippets = new List<Snippet>();

            snippets = ascQuery.Where(el => el.SnippetCreatedDateTime >= queryPivot.PivotDateTime && el.SnippetId > queryPivot.PivotId)
                    .Take(size).Reverse().ToList();

            if (snippets.Count < size)
            {
                var addToBottomSize = size - snippets.Count;

                var descQuery = _applicationDbContext.Snippets.Include(el => el.OwnerUser)
                    .OrderByDescending(el => el.SnippetCreatedDateTime).ThenByDescending(el => el.SnippetId).Where(el => el.Hidden == false);

                var bottomPartOfSnippets = await descQuery.Where(el => el.SnippetCreatedDateTime <= queryPivot.PivotDateTime && el.SnippetId <= queryPivot.PivotId)
                    .Take(addToBottomSize).ToListAsync();

                snippets.AddRange(bottomPartOfSnippets);
            }

            foreach (var snippet in snippets)
            {
                var stars = await GetSnippetStarsCountById(snippet.SnippetId.Value);
                snippet.SnippetStarsCount = stars;
                snippet.SnippetCommentsCount = 0;
            }

            return snippets;
        }

        public async Task<int> GetSnippetCommentsCountById(Guid snippetId)
        {
            return await _applicationDbContext
                .Comments.Where(el => el.CommentSnippetId == snippetId && el.Hidden == false).CountAsync();
        }

        public async Task<ICollection<Snippet>> GetSnippetsByOwnerId(Guid ownerId, int bodyLines)
        {
            if (bodyLines < 0)
            {
                throw new ArgumentException();
            }

            var snippets = _applicationDbContext.Snippets.Include(s => s.OwnerUser).Where(s => s.OwnerUserId == ownerId && s.Hidden == false);
            
            if (bodyLines > 0)
            {
                foreach (var snippet in snippets)
                {
                    snippet.SnippetBody = Utils.TrimToLines(snippet.SnippetBody, bodyLines);
                }
            }

            return await snippets.ToListAsync();
        }

        // Daha yenisi var mı
        public async Task<bool> IsExistNewerSnippet(QueryPivot queryPivot, Guid? ownerId)
        {
            var descQuery = _applicationDbContext.Snippets
                .OrderByDescending(el => el.SnippetCreatedDateTime).ThenByDescending(el => el.SnippetId).Where(el => el.Hidden == false);

            if (ownerId != null)
            {
                descQuery = descQuery.Where(el => el.OwnerUserId == ownerId);
            }

            var newerSnippetsCount = await descQuery.Where(
                el => el.SnippetCreatedDateTime >= queryPivot.PivotDateTime && el.SnippetId > queryPivot.PivotId)
                    .CountAsync();

            return newerSnippetsCount != 0;
        }

        // Daha eskisi var mı
        public async Task<bool> IsExistsOlderSnippet(QueryPivot queryPivot, Guid? ownerId)
        {
            var descQuery = _applicationDbContext.Snippets
                    .OrderByDescending(el => el.SnippetCreatedDateTime).ThenByDescending(el => el.SnippetId).Where(el => el.Hidden == false);

            if (ownerId != null)
            {
                descQuery = descQuery.Where(el => el.OwnerUserId == ownerId);
            }

            var olderSnippetsCount = await descQuery.Where(
                el => el.SnippetCreatedDateTime <= queryPivot.PivotDateTime && el.SnippetId < queryPivot.PivotId
                ).CountAsync();

            return olderSnippetsCount != 0;
        }

        public async Task<ICollection<Star>> GetOlderStarredSnippets(QueryPivot? queryPivot, int size, Guid starredUserId)
        {
            var descQuery = _applicationDbContext.Stars.Include(el => el.Snippet).Include(el => el.Snippet.OwnerUser).Include(el => el.OwnerUser)
                .OrderByDescending(el => el.LastUpdateTime).ThenByDescending(el => el.StarId)
                .Where(el => el.OwnerUserId == starredUserId && el.StarActive == true && el.Snippet.Hidden == false);

            var stars = new List<Star>();

            if (queryPivot == null)
            {
                stars = await descQuery.Take(size).ToListAsync();
            }
            else
            {
                stars = await descQuery.Where(el => el.LastUpdateTime <= queryPivot.PivotDateTime && el.StarId < queryPivot.PivotId).Take(size).ToListAsync();
                
                if (stars.Count < size)
                {
                    var addToTopSize = size - stars.Count;

                    var ascQuery = _applicationDbContext.Stars.Include(el => el.Snippet).Include(el => el.Snippet.OwnerUser).Include(el => el.OwnerUser)
                            .OrderBy(el => el.LastUpdateTime).ThenBy(el => el.StarId)
                            .Where(el => el.OwnerUserId == starredUserId && el.StarActive == true && el.Snippet.Hidden == false);

                    var topPartOfStars = await ascQuery.Where(el => el.LastUpdateTime >= queryPivot.PivotDateTime && el.StarId >= queryPivot.PivotId)
                        .Take(addToTopSize).Reverse().ToListAsync();

                    topPartOfStars.AddRange(stars);
                    stars = topPartOfStars;
                }
            }

            foreach (var star in stars)
            {
                var starsCount = await GetSnippetStarsCountById(star.Snippet.SnippetId.Value);
                var commentsCount = await GetSnippetCommentsCountById(star.Snippet.SnippetId.Value);
                star.Snippet.SnippetStarsCount = starsCount;
                star.Snippet.SnippetCommentsCount = commentsCount;
            }

            return stars;
        }

        public async Task<ICollection<Star>> GetNewerStarredSnippets(QueryPivot queryPivot, int size, Guid starredUserId)
        {
            var ascQuery = _applicationDbContext.Stars.Include(el => el.Snippet).Include(el => el.Snippet.OwnerUser).Include(el => el.OwnerUser)
                .OrderBy(el => el.LastUpdateTime).ThenBy(el => el.StarId)
                .Where(el => el.OwnerUserId == starredUserId && el.StarActive == true && el.Snippet.Hidden == false);


            var stars = new List<Star>();

            stars = ascQuery.Where(el => el.LastUpdateTime >= queryPivot.PivotDateTime && el.StarId > queryPivot.PivotId)
                    .Take(size).Reverse().ToList();

            if (stars.Count < size)
            {
                var addToBottomSize = size - stars.Count;

                var descQuery = _applicationDbContext.Stars.Include(el => el.Snippet).Include(el => el.Snippet.OwnerUser).Include(el => el.OwnerUser)
                    .OrderByDescending(el => el.LastUpdateTime).ThenByDescending(el => el.StarId)
                    .Where(el => el.OwnerUserId == starredUserId && el.StarActive == true && el.Snippet.Hidden == false);

                var bottomPartOfSnippets = await descQuery.Where(el => el.LastUpdateTime <= queryPivot.PivotDateTime && el.StarId <= queryPivot.PivotId)
                    .Take(addToBottomSize).ToListAsync();

                stars.AddRange(bottomPartOfSnippets);
            }

            foreach (var star in stars)
            {
                var starsCount = await GetSnippetStarsCountById(star.Snippet.SnippetId.Value);
                var commentsCount = await GetSnippetCommentsCountById(star.Snippet.SnippetId.Value);
                star.Snippet.SnippetStarsCount = starsCount;
                star.Snippet.SnippetCommentsCount = commentsCount;
            }

            return stars;
        }

        public async Task<bool> IsExistNewerStarredSnippet(QueryPivot pivot, Guid starredUserId)
        {
            var descQuery = _applicationDbContext.Stars.Include(el => el.Snippet)
                .OrderByDescending(el => el.LastUpdateTime).ThenByDescending(el => el.StarId)
                .Where(el => el.OwnerUserId == starredUserId && el.StarActive == true && el.Snippet.Hidden == false);

            var newerSnippetsCount = await descQuery.Where(
                el => el.LastUpdateTime >= pivot.PivotDateTime && el.StarId > pivot.PivotId)
                    .CountAsync();

            return newerSnippetsCount != 0;
        }

        public async Task<bool> IsExistOlderStarredSnippet(QueryPivot pivot, Guid starredUserId)
        {
            var descQuery = _applicationDbContext.Stars.Include(el => el.Snippet)
                .OrderByDescending(el => el.LastUpdateTime).ThenByDescending(el => el.StarId)
                .Where(el => el.OwnerUserId == starredUserId && el.StarActive == true && el.Snippet.Hidden == false);

            var olderSnippetsCount = await descQuery.Where(
                el => el.LastUpdateTime <= pivot.PivotDateTime && el.StarId < pivot.PivotId
                ).CountAsync();

            return olderSnippetsCount != 0;
        }
    }
}