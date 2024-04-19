using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class CategoryController : ControllerBase
    {
        private readonly IManageCategoryUseCase _manageCategory;

        public CategoryController(IManageCategoryUseCase manageCategory)
        {
            _manageCategory = manageCategory;
        }

        [Authorize(Policy = "IsAdminOrUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var data = await _manageCategory.GetAllCategoriesAsync();
            return Ok(data);
        }

        [Authorize(Policy = "IsAdminOrUser")]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {

            var dato = await _manageCategory.GetCategoryAsync(id);
            if (dato == null)
            {
                return NotFound();
            }
            return Ok(dato);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDTO modelo)
        {
            var result = await _manageCategory.AddCategoryAync(modelo);

            return Ok(modelo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO modelo)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            await _manageCategory.EditCategoryAsync(id, modelo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _manageCategory.RemoveCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
