using crud.Data;
using crud.Interface;
using crud.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace crud.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly appdbcontext _context;

        public UserRepository(appdbcontext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }
    }
}
