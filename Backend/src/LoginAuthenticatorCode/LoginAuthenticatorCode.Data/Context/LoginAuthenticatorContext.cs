using LoginAuthenticatorCode.Data.Mapping;
using LoginAuthenticatorCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginAuthenticatorCode.Data.Context;

    public class LoginAuthenticatorContext : DbContext
    {
        public LoginAuthenticatorContext(DbContextOptions<LoginAuthenticatorContext> options) : base(options){ }
        public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserMap).Assembly);
    }
}
    

