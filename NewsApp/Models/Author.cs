using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models;

public partial class Author
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string About { get; set; } = null!;
}
