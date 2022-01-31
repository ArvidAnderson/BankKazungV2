using BankKazungV2.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BankKazungV2.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Set email to unique constraint
            builder.Entity<User>()
              .HasIndex(User => User.Email)
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
