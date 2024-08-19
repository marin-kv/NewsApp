using NewsApp.Models;

namespace NewsApp.Repositories
{
    public interface IAuthorsRepository
    {
        Task<Author?> GetAuthor(int id);
        Task<Author?> GetAuthorByUserId(int userId);
        Task<int> CreateAuthor(Author author);
        Task UpdateAuthor(Author author);
        Task DeleteAuthor(int id);
        Task<bool> IsUserAuthor(int userId, int authorId);
    }
}
