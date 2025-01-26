using Microsoft.AspNetCore.Identity;

namespace ApiGrpc.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public virtual ICollection<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();

        public ApplicationUser()
        { }

        public ApplicationUser(string email, string firstName, string lastName)
        {
            Id = Guid.NewGuid();
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