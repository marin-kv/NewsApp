namespace NewsApp.Models.DTO.Article;

public class CreateArticleDto : ArticleDtoBase
{
    public DB.Article ToEntity(int authorId) => new()
    {
        AuthorId = authorId,
        Title = Title,
        Content = Content
    };
}
