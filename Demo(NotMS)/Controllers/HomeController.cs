using OnlineCourse.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace OnlineCourse.Controllers
{
    public class HomeController : Controller
    {
        IDbContext _context;
        public HomeController(IDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }
    }
}