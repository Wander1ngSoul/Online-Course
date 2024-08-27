using OnlineCourse.Entity.Models;

namespace OnlineCourse.Entity.Repositories.Interface
{
    public interface IUserRepositories
    {
        User GetUserByEmail(string email);
        void SaveUser(User user);
        User GetUserByLoginAndPassword(string email, string password);
        Task Register(string firstName, string lastName, decimal? stage, string role, string phone, string email, string password);
        Task Confirm(string email, string code);
        User GetUser(long id);       
        Task Edit(User user);
        Task Delete(string wmail);
    }
}
