using OnlineCourse.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCourse.Entity
{
    public interface IDbContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Class> Classes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        //int SaveChanges(CancellationToken cancellationToken = default);
    }
}
