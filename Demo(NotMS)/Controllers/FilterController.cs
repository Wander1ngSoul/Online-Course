using OnlineCourse.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using OnlineCourse.Entity.Models;

namespace OnlineCourse.Controllers
{
    public class FilterController : Controller
    {
        IDbContext _context;
        public FilterController(IDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Search(string term)
        {
            var results = _context.Courses.Where(x=>x.NameCourse.ToLower().Contains(term)).ToList();
            return Ok(results);
        }
       
    }
}