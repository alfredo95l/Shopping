using Microsoft.AspNetCore.Identity;
using Shoping.Data.Entities;
using Shoping.Models;

namespace Shoping.Helpers
{
    public interface IUserHelper
    {
        Task<User> AddUserAsync(AddUserViewModel model);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(Guid userId);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);


    }
}
