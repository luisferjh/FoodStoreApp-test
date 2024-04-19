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
    public class FoodController : ControllerBase
    {
        private readonly IManageFoodUseCase _manageFood;

        public FoodController(IManageFoodUseCase manageFood)
        {
            _manageFood = manageFood;
        }

        [Authorize(Policy = "IsAdminOrUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodDTO>>> Get()
        {
            var data = await _manageFood.GetAllFoodsInCatalogAsync();
            return Ok(data);
        }

        [Authorize(Policy = "IsAdminOrUser")]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<FoodDTO>> Get(int id)
        {
            var dato = await _manageFood.GetFoodInCatalogAsync(id);
            if (dato == null)
            {
                return NotFound();
            }
            return Ok(dato);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateFoodDTO modelo)
        {
            var result = await _manageFood.AddFoodToCatalogAsync(modelo);

            return Ok(modelo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreateFoodDTO modelo)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            await _manageFood.EditFoodInCatalogAsync(id, modelo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _manageFood.RemoveFoodFromCatalogAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
