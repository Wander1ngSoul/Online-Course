using OnlineCourse.Entity.Models;
using static IdentityServer4.Models.IdentityResources;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using OnlineCourse.Entity.Repositories.Interface;
using OnlineCourse.Services;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Entity.Repositories
{
    public class CartRepositories : ICartRepositories
    {
        private IDbContext _dbContext;
        private IEmailService _mailService;        

        public CartRepositories(IDbContext dbContext, IEmailService mailService)
        {
            _dbContext = dbContext;
            _mailService = mailService;
        }

        public async Task<bool> AddCart(Cart cart) {
            try
            {
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Cart? GetCart(string email, long courceId)
        {
            Cart cart = new Cart();
            var user = _dbContext.Users.FirstOrDefault(x => x.Email.ToLower() == email);   
            cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == user.Id && x.CourseId == courceId);     
            return cart;
        }


        public List<CourseDto> GetAllCources()
        {
            var list = _dbContext.Courses.Select(x=>new CourseDto()
            {
                Cost= x.Cost,
                CourseId= x.CourseId,
                Hours= x.Hours,
                inforamtion = x.inforamtion,
                NameCourse= x.NameCourse,
                Img = x.Img,
            }).ToList();
            return list;
        }
        public List<CourseDto> GetAllCourcesByUser(long? id, string? searchInput, string?  hoursFilter, string?  costFilter)
        {
            List<CourseDto> listResult = new List<CourseDto>();
            if (id == null)
            {
                listResult = GetAllCources();
            }
            else
            {
                listResult = _dbContext.Courses.Select(x => new CourseDto()
                {
                    Cost = x.Cost,
                    CourseId = x.CourseId,
                    Hours = x.Hours,
                    inforamtion = x.inforamtion,
                    NameCourse = x.NameCourse,
                    Img = x.Img,
                }).ToList();

                var listCart = _dbContext.Carts.Where(x => x.UserId == id).ToList();
                foreach (var cource in listResult)
                {
                    if (listCart.Any(x => x.CourseId == cource.CourseId))
                        cource.IsInCart = true;
                    else
                        cource.IsInCart = false;
                }
            }            
            return FilterCourses(listResult,  searchInput, hoursFilter, costFilter);
        }

        public List<Course> GetCourcesByUser(long? userId)
        {
            var listCourse = _dbContext.Courses.ToList();
            var listCart = _dbContext.Carts.Where(x => x.UserId == userId).Select(x=>x.CourseId).ToList();
            return _dbContext.Courses.Where(x=>listCart.Contains(x.CourseId)).ToList();
        }
        public async Task<bool> DeleteCart(long userId, long courceId)
        {
            try
            {
                Cart cart = new Cart();
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
                // Находим запись в корзине пользователя для данного курса           
                cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == user.Id && x.CourseId == courceId);
                // Если запись найдена, удаляем ее из базы данных
                if (cart != null)
                {
                    _dbContext.Carts.Remove(cart);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch 
            {
                return false;
            }           
        }

        public List<CourseDto> FilterCourses(List<CourseDto> courses, string searchInput, string hoursFilter, string costFilter)
        {            

            // Применяем фильтрацию поиска
            if (!string.IsNullOrEmpty(searchInput))
            {
                courses = courses.Where(c => c.NameCourse.ToLower().Contains(searchInput.ToLower())).ToList();
            }

            // Применяем фильтрацию по параметрам часов
            if (!string.IsNullOrEmpty(hoursFilter))
            {
                int hours = int.Parse(hoursFilter);
                courses = courses.Where(c => c.Hours <= hours).ToList();
            }

            // Применяем фильтрацию по параметрам стоимости
            if (!string.IsNullOrEmpty(costFilter))
            {
                int cost = int.Parse(costFilter);
                courses = courses.Where(c => c.Cost <= cost).ToList();
            }
            // Возвращаем представление с отфильтрованными курсами
            return courses;
        }

    }
}
