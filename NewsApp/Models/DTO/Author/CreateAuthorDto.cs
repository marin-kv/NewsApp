namespace NewsApp.Models.DTO.Author;

public class CreateAuthorDto : AuthorDtoBase
{
    public DB.Author ToEntity() => new()
    {
        FirstName = FirstName,
        LastName = LastName,
        About = About
    };
}
