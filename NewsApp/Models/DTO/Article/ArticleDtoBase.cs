namespace NewsApp.Models.DTO.Article;

public class ArticleDtoBase
{
    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
