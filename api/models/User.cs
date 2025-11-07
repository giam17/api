using System.ComponentModel.DataAnnotations;

namespace api.models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
