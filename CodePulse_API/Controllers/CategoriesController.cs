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

    }
}
