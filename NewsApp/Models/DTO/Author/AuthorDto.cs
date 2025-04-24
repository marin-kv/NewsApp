namespace NewsApp.Models.DTO.Author;

public class AuthorDto : AuthorDtoBase
{
    public int Id { get; set; }

    public AuthorDto FromEntity(DB.Author author)
    {
        Id = author.Id;
        FirstName = author.FirstName;
        LastName = author.LastName;
        About = author.About;
        return this;
    }
}
