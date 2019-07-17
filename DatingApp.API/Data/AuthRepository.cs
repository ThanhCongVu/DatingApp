using System;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public Task<User> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(User user, string password)
        {
            // we dont want to store the password in clear text
            // we need to convert the password to PasswordHash with PasswordSalt
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            // we want to pass passwordHash and PasswordSalt as a reference, not as a value
            // so that we use keyword "out"

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // In this project, we use cryptography as method to hash our password
            // Since Cryptography.HMACSHA512 provide random generated key so we can use it as passwrodSalt

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                // method computeHash takes bytes[] so we need to convert our password to byte
            }

        }

        public Task<bool> UserExists(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}