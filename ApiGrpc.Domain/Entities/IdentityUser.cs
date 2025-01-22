using Microsoft.AspNetCore.Identity;

namespace ApiGrpc.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public ApplicationUser()
        { } // EF Core

        public ApplicationUser(string email, string firstName, string lastName)
        {
            Email = email;
            UserName = email;
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = DateTime.UtcNow;
        }

        public async Task<bool> IsInRoleAsync(UserManager<ApplicationUser> userManager, string role)
        {
            return await userManager.IsInRoleAsync(this, role);
        }
    }
}