using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Models;
using NewsApp.Repositories;

namespace NewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesController(IArticlesRepository articlesRepository) : ControllerBase
    {
        private readonly IArticlesRepository _articlesRepository = articlesRepository;

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            var article = await _articlesRepository.GetArticle(id);
            return Ok(article);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var articles = await _articlesRepository.GetArticles(page, pageSize);
            return Ok(articles);
        }

        [HttpGet]
        [Route("search/{searchTerm}")]
        public async Task<IActionResult> SearchArticles([FromRoute] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var articles = await _articlesRepository.SearchArticlesByTitle(searchTerm, page, pageSize);
            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            var articleId = await _articlesRepository.CreateArticle(article);
            return Ok(articleId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArticle(Article article)
        {
            await _articlesRepository.UpdateArticle(article);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            await _articlesRepository.DeleteArticle(id);
            return Ok();
        }
    }
}
