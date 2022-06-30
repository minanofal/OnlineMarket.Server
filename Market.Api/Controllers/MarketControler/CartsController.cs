using Market.Core.AuthenticationsModels;
using Market.Core.InterFaces.MarketInterfaces;
using Market.Core.Models.MarketModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Market.Api.Controllers.MarketControler
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManger;

        public CartsController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManger)
        {
            _userManger = userManger;
            _unitOfWork = unitOfWork;

        }
        [HttpPost("AddCart")]
        public async Task<IActionResult> adddCart([FromBody] AddCartDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
           var result = await _unitOfWork.Carts.AddToCart(dto);
            if(!result)
                return BadRequest(result);
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> getUserCart()
        {
            var u = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var s = await _userManger.FindByNameAsync(u);
            var result = await _unitOfWork.Carts.GetCartProducts( s.Id);
            if (result == null)
                return NotFound("There is no user with this id");
            return Ok(result);
        }

        [HttpGet("count/{id}")]
        public async Task<IActionResult> getUserCartcount(string id)
        {
            var result = await _unitOfWork.Carts.GetCartProducts(id);
            if (result == null)
                return NotFound("There is no user with this id");
            return Ok(result.Count());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteCart(Guid id)
        {
            var result = await _unitOfWork.Carts.RemoveFromCart(id);
            if (!result)
                return NotFound("There is no user with this id");
            return Ok(result);
        }

    }
}
