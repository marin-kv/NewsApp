using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Models;
using NewsApp.Repositories;
using System.Security.Claims;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController(IAuthorsRepository authorRepository, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IAuthorsRepository _authorRepository = authorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAuthor([FromRoute] int id)
        {
            var author = await _authorRepository.GetAuthor(id);
            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthorFromCurentUser(Author author)
        {
            var userIdString = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);
            author.UserId = userId;
            var authorId = await _authorRepository.CreateAuthor(author);
            return Ok(authorId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAuthor(Author author)
        {     
            await _authorRepository.UpdateAuthor(author);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
        {
            await _authorRepository.DeleteAuthor(id);
            return Ok();
        }
    }
}
