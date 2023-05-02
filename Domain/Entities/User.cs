using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public static User Create(string name, string password)
        {
            return new User
            {
                Name = name,
                Password = (new PasswordHasher<object?>()).HashPassword(null, password)
            };
        }
    }
}
