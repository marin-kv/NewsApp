using Microsoft.EntityFrameworkCore;
using NewsApp.Models;

namespace NewsApp.Repositories.Implementation
{
    public class ArticlesRepository(NewsDbContext context) : IArticlesRepository
    {
        private readonly NewsDbContext _context = context;

        public async Task<Article?> GetArticle(int id)
        {
            return await _context.Articles.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Article>> GetArticles(int page, int pageSize)
        {
            return await _context.Articles.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesForAuthor(int authorId, int page, int pageSize)
        {
            return await _context.Articles.Where(a => a.AuthorId == authorId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Article>> SearchArticlesByTitle(string searchTerm, int page, int pageSize)
        {
            return await _context.Articles.Where(a => a.Title.Contains(searchTerm)).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> CreateArticle(Article article)
        {
            var articleEntity = _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return articleEntity.Entity.Id;
        }

        public async Task UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticle(int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(a => a.Id == id);
            if (article is not null) _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}
