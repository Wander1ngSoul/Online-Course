using System.Threading.Tasks;
using OnlineCourse.Entity.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineCourse.Entity;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;



namespace OnlineCourse.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private IDbContext _dbContext;

        public EmailService(IOptions<EmailSettings> emailSettings, IDbContext dbContext)
        {
            _emailSettings = emailSettings.Value;
            _dbContext = dbContext;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = message
                };

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, such as logging the error
                throw;
            }
        }
        public async Task SendConfirmationEmailAsync(long userId, string email)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }
            // Генерация токена подтверждения почты
            var token = GenerateEmailConfirmationToken();
            user.EmailConfirmationToken = token;
            await _dbContext.SaveChangesAsync();

            // Формирование ссылки для подтверждения почты
            var confirmationLink = $"https://localhost:7145/Auth/Confirm?email={email}&code={token}";

            await SendEmailAsync(email, "Подтверждение почты",$"Для подтверждения вашей почты перейдите по ссылке: <a href=\"{confirmationLink}\">Подтвердить</a>");
           
        }

        private string GenerateEmailConfirmationToken()
        {
            // Генерация случайного токена
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return WebEncoders.Base64UrlEncode(tokenBytes);
        }
    }
}