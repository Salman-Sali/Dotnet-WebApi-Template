using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Context.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(a => a.Name)
                .IsUnique();

            builder
                .HasData(
                    User.Create("asdfasdf", "asdfasdfas")
                );
        }
    }
}
