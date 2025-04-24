namespace NewsApp.Models.DTO.Article;

public class CreateArticleDto : ArticleDtoBase
{
    public DB.Article ToEntity() => new()
    {
        AuthorId = AuthorId,
        Title = Title,
        Content = Content
    };
}
