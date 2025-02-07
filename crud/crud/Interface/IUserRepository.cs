using crud.Model.Entities;

namespace crud.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(User user);  // Adds a new user
        Task<User> GetByIdAsync(int userId);  // Retrieves a user by ID
        Task<IEnumerable<User>> GetAllAsync();  // Retrieves all users
        Task UpdateAsync(User user);  // Updates a user's information
        Task DeleteAsync(int userId);  // Deletes a user
    }
}
