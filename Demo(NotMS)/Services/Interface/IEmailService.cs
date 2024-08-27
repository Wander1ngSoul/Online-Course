namespace OnlineCourse.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendConfirmationEmailAsync(long userId, string email);
    }
}
