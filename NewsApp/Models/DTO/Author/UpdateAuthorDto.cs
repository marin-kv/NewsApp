namespace NewsApp.Models.DTO.Author;

public class UpdateAuthorDto : AuthorDtoBase
{
    public int Id { get; set; }

    public DB.Author ToEntity() => new()
    {
        Id = Id,
        FirstName = FirstName,
        LastName = LastName,
        About = About
    };
}
