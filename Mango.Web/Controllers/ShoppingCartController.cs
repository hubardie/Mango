using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto response = await _shoppingCartService.RemoveFromCartAsync(cartDetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Cart updated succesfully";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            else
            {
                TempData["Error"] = response?.Message;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto carDto)
        {
            ResponseDto response = await _shoppingCartService.ApplyCouponAsync(carDto);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Coupon applied";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            else
            {
                TempData["Error"] = response?.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto carDto)
        {
            carDto.CartHeader.CouponCode = "";
            ResponseDto response = await _shoppingCartService.ApplyCouponAsync(carDto);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Coupon removed";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            else
            {
                TempData["Error"] = response?.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto carDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
           cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            ResponseDto response = await _shoppingCartService.EmailCart(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Email will be processed and sent shortly.";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            else
            {
                TempData["Error"] = response?.Message;
                return View();
            }
        }
        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto response = await _shoppingCartService.GetCartByUserIdAsync(userId);
            CartDto cartDto = new CartDto();
            if (response != null && response.IsSuccess)
            {
                cartDto = Newtonsoft.Json.JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            return cartDto;
        }
    }
}
