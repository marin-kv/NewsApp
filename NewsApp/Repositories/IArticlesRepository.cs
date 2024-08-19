using NewsApp.Models;

namespace NewsApp.Repositories
{
    public interface IArticlesRepository
    {
        Task<Article?> GetArticle(int id);
        Task<IEnumerable<Article>> GetArticles(int page, int pageSize);
        Task<IEnumerable<Article>> GetArticlesForAuthor(int authorId, int page, int pageSize);
        Task<IEnumerable<Article>> SearchArticlesByTitle(string searchTerm, int page, int pageSize);
        Task<int> CreateArticle(Article article);
        Task UpdateArticle(Article article);
        Task DeleteArticle(int id);
        Task<bool> CanUserModifyArticle(int userId, int articleId);
    }
}
