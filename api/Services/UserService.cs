using api.Data;
using api.models;
using api.models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace api.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _appDbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> RegisterUserAsync(RegisterUserDto dto)
        {
            if (await _appDbContext.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email ya registrado");

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = HashPassword(dto.Password)
            };

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            if (!string.IsNullOrWhiteSpace(dto.Email) && !dto.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailEnUso = await _appDbContext.Users.AnyAsync(u => u.Email == dto.Email && u.Id != user.Id);
                if (emailEnUso)
                    throw new Exception("El email ya está en uso por otro usuario");

                user.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = HashPassword(dto.Password);

            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
