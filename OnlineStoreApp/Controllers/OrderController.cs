using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsUser")]
    public class OrderController : ControllerBase
    {
        private readonly IMakeOrderUseCase _orderUC;

        public OrderController(IMakeOrderUseCase OrderUC)
        {
            _orderUC = OrderUC;
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var data = await _orderUC.GetAllAsync();
            return Ok(data);
        }


        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OrderRequestDTO model)
        {
            var result = await _orderUC.PlaceOrderAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Message);
        }
    }
}
