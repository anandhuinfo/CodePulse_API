using CodePulse_API.Migrations;
using CodePulse_API.Models.Domain;
using CodePulse_API.Models.DTO;
using CodePulse_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CodePulse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository) {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO request) {

            var blogPost = new Models.Domain.BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories) {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null) {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            var response = new BlogPostResponseDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> getAllBlogPost() {
            var blogPosts = await blogPostRepository.GetAllBlogPostAsync();

            var response = new List<BlogPostResponseDTO>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostResponseDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()

                });
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> deleteBlogPost([FromRoute]Guid id) {

            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost == null)
                return NotFound();

            var response = new BlogPostResponseDTO
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                PublishedDate = deletedBlogPost.PublishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                Title = deletedBlogPost.Title,
                UrlHandle = deletedBlogPost.UrlHandle
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostbyUrl([FromRoute] string urlHandle) { 
        
            var blogPost = await blogPostRepository.GetbyUrlHandleAsync(urlHandle);

            if (blogPost == null)
                    return NotFound();

            var response = new BlogPostResponseDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()

            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostbyId([FromRoute] Guid id)
        {

            var blogPost = await blogPostRepository.GetbyIdAsync(id);

            if (blogPost == null)
                return NotFound();

            var response = new BlogPostResponseDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()

            };
            return Ok(response);
        }

    }
}
