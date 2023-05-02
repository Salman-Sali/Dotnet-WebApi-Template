using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.ServiceInterfaces
{
    public interface IMainDbContext
    {
        public DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
