using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NewsApp.Exceptions;
using NewsApp.Models.DTO.Article;
using NewsApp.Repositories;
using System.Security.Claims;

namespace NewsApp.Controllers
{
    //TODO: Move logic to service layer to separate concerns

    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController(IArticlesRepository articlesRepository, IAuthorsRepository authorsRepository) : ControllerBase
    {
        private readonly IArticlesRepository _articlesRepository = articlesRepository;
        private readonly IAuthorsRepository _authorsRepository = authorsRepository;

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            var article = await _articlesRepository.GetArticle(id);

            if (article is null)
            {
                throw new NotFoundException("Article not found");
            }

            var articleDto = new ArticleDto().FromEntity(article);

            return Ok(articleDto);
        }

        [HttpGet]
        [OutputCache(Duration = 30)] // TODO: Limit cache to first page (for theoretical homepage)
        public async Task<IActionResult> GetArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var articles = await _articlesRepository.GetArticles(page, pageSize);
            var articleDtos = articles.Select(article => new ArticleDto().FromEntity(article)).ToList();
            return Ok(articleDtos);
        }

        [HttpGet]
        [Route("search/{searchTerm}")]
        public async Task<IActionResult> SearchArticles([FromRoute] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var articles = await _articlesRepository.SearchArticlesByTitle(searchTerm, page, pageSize);
            var articleDtos = articles.Select(article => new ArticleDto().FromEntity(article)).ToList();
            return Ok(articleDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateArticle(CreateArticleDto article)
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);
            var author = await _authorsRepository.GetAuthorByUserId(userId);

            if (author is null)
            {
                throw new BadRequestException("You are not an author");
            }

            var articleEntity = article.ToEntity(author.Id); 
            var articleId = await _articlesRepository.CreateArticle(articleEntity);
            return Ok(articleId);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(UpdateArticleDto article)
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);
            if (!await _articlesRepository.CanUserModifyArticle(userId, article.Id))
            {
                return Forbid();
            }

            var articleEntity = article.ToEntity();

            await _articlesRepository.UpdateArticle(articleEntity);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            var userIdString = HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userId = int.Parse(userIdString);
            if (!await _articlesRepository.CanUserModifyArticle(userId, id))
            {
                return Forbid();
            }
            await _articlesRepository.DeleteArticle(id);
            return Ok();
        }
    }
}
