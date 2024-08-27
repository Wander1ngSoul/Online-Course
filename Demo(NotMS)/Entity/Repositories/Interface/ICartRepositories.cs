using OnlineCourse.Entity.Models;

namespace OnlineCourse.Entity.Repositories.Interface
{
    public interface ICartRepositories
    {
        Task<bool> AddCart(Cart cart);        
        Cart? GetCart(string email, long courceId);
        Task<bool> DeleteCart(long userId, long courceId);
        List<CourseDto> GetAllCources();
        List<CourseDto> GetAllCourcesByUser(long? id, string? searchInput, string? hoursFilter, string? costFilter);
        List<Course> GetCourcesByUser(long? id);       
    }
}
