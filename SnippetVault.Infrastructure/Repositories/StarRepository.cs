using Microsoft.EntityFrameworkCore;
using SnippetVault.Core.Domain.Entities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Infrastructure.DbContext;


namespace SnippetVault.Infrastructure.Repositories
{
    public class StarRepository : IStarRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StarRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Star> AddStar(Star star)
        {
            star.LastUpdateTime = DateTime.UtcNow;

            await _applicationDbContext.Stars.AddAsync(star);
            await _applicationDbContext.SaveChangesAsync();
            return star;
        }

        public async Task<bool> DeleteStarById(Guid starId)
        {
            _applicationDbContext.Stars.RemoveRange(_applicationDbContext.Stars.Where(el => el.StarId == starId));
            var rowsDeleted = await _applicationDbContext.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<Star> GetStarById(Guid starId)
        {
            return await _applicationDbContext.Stars.FirstAsync(el => el.StarId == starId);
        }

        public async Task<Star?> GetStarByOwnerIdAndSnippetId(Guid ownerUserId, Guid snippetId)
        {
            return await _applicationDbContext.Stars.FirstOrDefaultAsync(el => el.SnippetId == snippetId && el.OwnerUserId == ownerUserId);
        }

        public async Task<Star> UpdateStar(Star star)
        {
            var found = await _applicationDbContext.Stars.FirstAsync(el => el.StarId == star.StarId);
            found.LastUpdateTime = DateTime.UtcNow;
            found.StarActive = star.StarActive;
            await _applicationDbContext.SaveChangesAsync();

            return found;
        }
    }
}
