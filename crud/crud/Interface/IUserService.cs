using crud.Model.Entities;
using crud.Model.Request;

namespace crud.Interface
{
    public interface IUserService
    {
        Task<bool> SaveUser(userdto request);
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> UpdateUser(int id, userdto request);
        Task<bool> DeleteUser(int id);
        Task<string> GenerateToken(int id);
    }
}
