using Microsoft.EntityFrameworkCore;
using Todo.API.Entities;

namespace Todo.API.Infrastructure
{
    public class TodoDbContext : DbContext
    {
        public DbSet<TodoTask> TodoTasks { get; set; }
        public DbSet<User> Users { get; set; }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }
    }
}