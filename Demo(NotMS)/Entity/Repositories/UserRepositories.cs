using OnlineCourse.Entity.Models;
using static IdentityServer4.Models.IdentityResources;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using OnlineCourse.Entity.Repositories.Interface;
using OnlineCourse.Services;


namespace OnlineCourse.Entity.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private IDbContext _dbContext;
        private IEmailService _mailService;

        public UserRepositories(IDbContext dbContext, IEmailService mailService)
        {
            _dbContext = dbContext;
            _mailService = mailService;
        }

        public async void SaveUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        public User GetUser(long id)
        {
            User User = new User();
            try
            {
                User = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            }

            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }

            return User;
        }
        public User GetUserByEmail(string email)
        {
            User User = new User();
            try
            {
                User = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            }

            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }

            return User;
        }
        public User GetUserByLoginAndPassword(string email, string password)
        {
            try
            {
                User user = new User();
                user = _dbContext.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
                if (user != null && UserService.VerifyPassword(password, user.Password))
                    return user;
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }
            return null;
        }
        public async Task Register(string firstName, string lastName, decimal? stage, string role, string phone, string email, string password)
        {
            User user = new User();
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new InvalidOperationException("Пользователь с таким email уже зарегистрирован.");
            }
            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Stage = stage,
                Role = role,
                Phone = phone,
                Email = email,
                Password = UserService.HashPassword(password),
                ConfirmEmail = false
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            await _mailService.SendConfirmationEmailAsync(newUser.Id, email);
        }
        public async Task Confirm(string email, string code)
        {
            try
            {
                var users = _dbContext.Users.ToList();
                var user = _dbContext.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.EmailConfirmationToken == code);
                if (user == null)
                {
                    throw new InvalidOperationException("Пользователь не найден.");
                }
                user.ConfirmEmail = true;
                await _dbContext.SaveChangesAsync();
            }

            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }
        }    

        public async Task Edit(User data)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == data.Email);
            if (user != null)
            {
                if (user.FirstName != data.FirstName)
                    user.FirstName = data.FirstName;
                if (user.LastName != data.LastName)
                    user.LastName = data.LastName;
                if (user.Email != data.Email)
                    user.Email = data.Email;
                if (user.Stage != data.Stage)
                    user.Stage = data.Stage;
                if (user.Role != data.Role)
                    user.Role = data.Role;
                if (user.Phone != data.Phone)
                    user.Phone = data.Phone;
                if (data.Password != null && user.Password != UserService.HashPassword(data.Password) && user.Password != data.Password)
                    user.Password = UserService.HashPassword(data.Password);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task Delete(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
