using NewsApp.Models;

namespace NewsApp.Repositories
{
    public interface IAuthorsRepository
    {
        Task<Author?> GetAuthor(int id);
        Task<int> CreateAuthor(Author author);
        Task UpdateAuthor(Author author);
        Task DeleteAuthor(int id);
    }
}
