namespace OnlineCourse.Entity.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Stage { get; set; }
        public string Role { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? ConfirmEmail { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
