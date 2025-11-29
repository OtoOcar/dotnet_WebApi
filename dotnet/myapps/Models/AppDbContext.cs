using Microsoft.EntityFrameworkCore;

namespace libreria.WebApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Users>(e =>
            {
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Email).IsRequired();
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.LastName).IsRequired();
                e.Property(x => x.Password).IsRequired();
            });

            b.Entity<Book>(e =>
            {
                e.HasOne(x => x.User)
                 .WithMany(u => u.Books)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.Title).IsRequired();
                e.Property(x => x.Author).IsRequired();
                e.Property(x => x.PublishYear).IsRequired();
                e.Property(x => x.CoverUrl).IsRequired(false);
                e.Property(x => x.Rating).IsRequired(false);
                e.Property(x => x.Comment).IsRequired(false);

            });

        }
    }
}