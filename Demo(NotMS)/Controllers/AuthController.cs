using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication;
using OnlineCourse.Entity.Models;
using static IdentityServer4.Models.IdentityResources;
using OnlineCourse.Entity.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace OnlineCourse.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepositories _userRepositories;

        public AuthController(IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string firstName, string lastName, decimal? stage, string role, string phone, string email, string password)
        {
            try
            {
                await _userRepositories.Register(firstName, lastName, stage, role, phone, email, password);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(TempData);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Confirm(string email, string code)
        {
            _userRepositories.Confirm(email, code);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl)
        {
            try
            {
                // находим пользователя           
                User user = _userRepositories.GetUserByLoginAndPassword(email, password);
                // если пользователь не найден, отправляем статусный код 401
                if (user is null)
                {
                    TempData["ErrorMessage"] = ("Invalid username or password");
                    return View(TempData);
                }
                var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            };
                // создаем объект ClaimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Internal Error ";
                return View(TempData);
            }
            return Redirect(returnUrl ?? "/");
        }

        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #region pages
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Profile(long? id)
        {
            User user;
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (id.HasValue)
            {
                user = _userRepositories.GetUser(id.Value);
            }
            else
                user = _userRepositories.GetUserByEmail(email);
            return View(user);
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> ProfileAsync(User updatedUser)
        {
            await _userRepositories.Edit(updatedUser);
            //return RedirectToAction("Profile");
            return View(_userRepositories.GetUserByEmail(updatedUser.Email));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            await _userRepositories.Delete(email);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // После удаления пользователя, например, перенаправляем на главную страницу
            return RedirectToAction("Index", "Home");
        }
    }
}
          

    

