using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }
        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }
        [HttpPost]
        [ActionName("CheckOut")]
        public async Task<IActionResult> CheckOut(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Name = cartDto.CartHeader.Name;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
                // get stripe session and redirect to stripe to place order
                // de stripe necesitaremos: Session Id, URL, Back/Cancel URL y Approved URL (cuando se haya procesado el pago ok)
                cartDto = Newtonsoft.Json.JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            return View();
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
