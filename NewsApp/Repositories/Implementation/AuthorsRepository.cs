using Microsoft.EntityFrameworkCore;
using NewsApp.Database;
using NewsApp.Models;

namespace NewsApp.Repositories.Implementation
{
    public class AuthorsRepository(NewsDbContext context) : IAuthorsRepository
    {
        private readonly NewsDbContext _context = context;

        public async Task<Author?> GetAuthor(int id)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author?> GetAuthorByUserId(int userId)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<int> CreateAuthor(Author author)
        {
            var authorEntity = _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return authorEntity.Entity.Id;
        }

        public async Task UpdateAuthor(Author author)
        {
            if (!_context.Authors.Any(a => a.Id == author.Id))
            {
                throw new BadHttpRequestException("Author doesn't exists", 400);
            }
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await _context.Authors.SingleOrDefaultAsync(a => a.Id == id);
            _context.Authors.Remove(author!);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserAuthor(int userId, int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.Id == authorId && a.UserId == userId);
        }
    }
}
