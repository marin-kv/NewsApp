namespace NewsApp.Models.DTO.Article;

public class UpdateArticleDto : ArticleDtoBase
{
    public int Id { get; set; }

    public DB.Article ToEntity() => new()
    {
        Id = Id,
        AuthorId = AuthorId,
        Title = Title,
        Content = Content
    };
}
