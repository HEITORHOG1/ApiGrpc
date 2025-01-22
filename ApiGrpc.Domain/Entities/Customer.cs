using ApiGrpc.Domain.Entities.Base;

namespace ApiGrpc.Domain.Entities
{
    public class Customer : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public bool Active { get; private set; }

        protected Customer()
        { } // EF Constructor

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Active = true;
        }

        public void Update(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            Active = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}