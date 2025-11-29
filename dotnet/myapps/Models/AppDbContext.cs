using Microsoft.EntityFrameworkCore;

namespace libreria.WebApi.Models
{
  public class AppDbContext : DbContext
  {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }   

}
}
