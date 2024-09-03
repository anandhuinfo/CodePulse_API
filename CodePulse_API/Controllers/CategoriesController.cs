using CodePulse_API.Data;
using CodePulse_API.Models.Domain;
using CodePulse_API.Models.DTO;
using CodePulse_API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse_API.Controllers
{
    // endPoint: https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
       
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDTO requestCategory) { 

            // Map DTO to Domain
            var category = new Category { Name = requestCategory.Name, UrlHandle = requestCategory.UrlHandle };
            await _categoryRepository.CreateAsync(category);
            //Domain Model to DTO
            var response = new CreateCategoryRequestDTO { Name= requestCategory.Name ,UrlHandle = requestCategory.UrlHandle };
            return Ok(response);
        }


        //https://localhost:7144/api/Categories
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllCategories() { 

           var categories = await _categoryRepository.GetAllAsync();

            // Map Domain model to DTO
            var response = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
                return Ok(response);
        }

        //https://localhost:7144/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> GetCategoryId([FromRoute] Guid id) {
            var existingCategory = await _categoryRepository.GetById(id);

            if (existingCategory is null)
                return NotFound();

            var response = new CategoryDTO
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto requestDto) {

            var category = new Category
            {
                Id = id,
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle
            };

          category = await _categoryRepository.UdpateCategoryAsync(category);

            if (category == null) {
                return NotFound();
            }

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id) {
            var category = await _categoryRepository.DeleteCategoryAsync(id);
            if (category is null)
                return NotFound();

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

    }
}
