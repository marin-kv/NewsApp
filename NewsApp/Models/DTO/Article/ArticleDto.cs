namespace NewsApp.Models.DTO.Article;

public class ArticleDto : ArticleDtoBase
{
    public int Id { get; set; }

    public ArticleDto FromEntity(DB.Article article)
    {
        Id = article.Id;
        AuthorId = article.AuthorId;
        Title = article.Title;
        Content = article.Content;
        return this;
    }
}
