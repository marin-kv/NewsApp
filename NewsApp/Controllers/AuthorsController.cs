using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Exceptions;
using NewsApp.Models.DTO.Author;
using NewsApp.Repositories;
using System.Security.Claims;

namespace NewsApp.Controllers
{
    //TODO: Move logic to service layer to separate concerns

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController(IAuthorsRepository authorRepository) : ControllerBase
    {
        private readonly IAuthorsRepository _authorRepository = authorRepository;

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAuthor([FromRoute] int id)
        {
            var author = await _authorRepository.GetAuthor(id);

            if (author is null)
            {
                throw new NotFoundException("Author does not exist");
            }

            var authorDto = new AuthorDto().FromEntity(author);

            return Ok(authorDto);
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetAuthorForCurrentUser()
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);
            var author = await _authorRepository.GetAuthorByUserId(userId);

            if (author is null)
            {
                throw new BadRequestException("You are not an author");
            }

            var authorDto = new AuthorDto().FromEntity(author);

            return Ok(authorDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthorFromCurentUser(CreateAuthorDto author)
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);

            if (await _authorRepository.GetAuthorByUserId(userId) is not null)
            {
                throw new BadRequestException("You are already an author");
            }

            var authorEntity = author.ToEntity();

            authorEntity.UserId = userId;
            var authorId = await _authorRepository.CreateAuthor(authorEntity);
            return Ok(authorId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAuthor(UpdateAuthorDto author)
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);

            if (await _authorRepository.GetAuthor(author.Id) is null)
            {
                throw new NotFoundException("Author does not exist");
            }

            if (!await _authorRepository.IsUserAuthor(userId, author.Id))
            {
                return Forbid();
            }

            var authorEntity = author.ToEntity();

            await _authorRepository.UpdateAuthor(authorEntity);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);

            if (await _authorRepository.GetAuthorByUserId(userId) is null)
            {
                throw new NotFoundException("Author does not exist");
            }

            if (!await _authorRepository.IsUserAuthor(userId, id))
            {
                return Forbid();
            }
            await _authorRepository.DeleteAuthor(id);
            return Ok();
        }
    }
}
