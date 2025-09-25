using System.ComponentModel.DataAnnotations;

namespace LocaRider.Domain.Entities.Users
{
    public class User
    {
        [Key]
        public Guid UserId { get; private set; }
        public string FullName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        private User() { }

        public User(string fullName, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email é obrigatório.");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Senha é obrigatória.");

            UserId = Guid.NewGuid();
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string fullName, string email, string? passwordHash = null)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email é obrigatório.");

            FullName = fullName;
            Email = email;

            if (!string.IsNullOrWhiteSpace(passwordHash))
                PasswordHash = passwordHash;
        }

        public void ChangePassword(string newPassword)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        }
    }
}
