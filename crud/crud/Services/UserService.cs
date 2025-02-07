using crud.Interface;
using crud.Model.Entities;
using crud.Model.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace crud.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> SaveUser(userdto request)
        {
            try
            {
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Age = request.Age,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                // Use the UserRepository to add the user
                await _unitOfWork.UserRepository.AddAsync(user);

                // Commit the transaction
                await _unitOfWork.CommitAsync();

                return true; // Success
            }
            catch (Exception)
            {
                // Handle exceptions, possibly roll back if needed
                await _unitOfWork.RollbackAsync();
                return false; // Error occurred during save
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                // Fetch all users via the repository
                return await _unitOfWork.UserRepository.GetAllAsync();
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., log them)
                return Enumerable.Empty<User>();  // Return an empty list in case of failure
            }
        }

        public async Task<bool> UpdateUser(int id, userdto request)
        {
            try
            {
                // Retrieve the existing user
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return false;
                }

                // Update properties from the DTO
                user.Name = request.Name;
                user.Email = request.Email;
                user.Age = request.Age;
                user.UpdatedDate = DateTime.UtcNow;

                // Update the user and commit changes
                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                // Retrieve the existing user
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return false;
                }

                // Delete the user and commit changes
                await _unitOfWork.UserRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<string> GenerateToken(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Name), // Ensure `user.Username` exists in your model
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim("Id",user.Id.ToString()),    
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
