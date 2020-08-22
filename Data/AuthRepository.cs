using System;
using System.Threading.Tasks;
using DesafioBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username) ? true : false;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users?.FirstOrDefaultAsync(x => x.UserName == username);
            return !VerifyPasswordHash(password, user.PasswordHash,user.PassowordSalt) ? null : user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passowordSalt)
        {
              using (var hmac = new System.Security.Cryptography.HMACSHA512(passowordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < ComputeHash.Length; i++)
                {
                    if(ComputeHash[i] != passwordHash[i]){
                        return false;
                    }
                } 
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
           byte[] passwordHash, passwordSalt;
           CreatePasswordHash(password, out passwordHash, out passwordSalt);
           user.PassowordSalt = passwordSalt;
           user.PasswordHash = passwordHash;
           
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();
           return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
