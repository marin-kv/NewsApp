using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models;

public partial class Article
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
