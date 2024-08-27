using OnlineCourse.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Entity.Models;
using System.Security.Claims;
using OnlineCourse.Entity.Repositories.Interface;
using OnlineCourse.Entity.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCourse.Controllers
{
    public class CartController : Controller
    {
       
        private readonly IUserRepositories _userRepositories;
        private readonly ICartRepositories _cartRepositories;
        public CartController(IUserRepositories userRepositories, ICartRepositories cartRepositories)
        {    
            _userRepositories = userRepositories;
            _cartRepositories = cartRepositories;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CourseInfoWindow()
        {                 
                return View();
        }
        public ActionResult GetCourseCards(string? searchInput, string? hoursFilter, string? costFilter)
        {
            long? id = null;
            if (User.Identity.IsAuthenticated)
            {
                id = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }    
            return PartialView("_CourseCards", _cartRepositories.GetAllCourcesByUser(id, searchInput, hoursFilter, costFilter));
        }

        [Authorize]
        public IActionResult Cart()
        {
            return View(_cartRepositories.GetCourcesByUser(long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartAsync(long courseId)
        {
            // Получаем текущего пользователя
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // Проверяем, есть ли уже этот курс в корзине пользователя
            var existingCartItem = _cartRepositories.GetCart(userEmail, courseId);//_dbContext.Cart.FirstOrDefault(c => c.Id == userId && c.CourseId == courseId);
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existingCartItem == null)
            {
                // Если курс еще не добавлен в корзину, добавляем новую запись
                await _cartRepositories.AddCart(new Cart() { CourseId = courseId, UserId = userId });               
            }
            // Перенаправляем пользователя обратно на страницу с курсами
            return RedirectToAction("CourseInfoWindow", "Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int courseId)
        {
            // Получаем текущего пользователя
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);           
            _cartRepositories.DeleteCart(long.Parse(userId), courseId);
            // Перенаправляем пользователя обратно на страницу с курсами
            return RedirectToAction("CourseInfoWindow", "Cart");
        }       
    }
}