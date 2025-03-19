using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data
{
    public class TimeTrackerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure SQLite as the database provider
            optionsBuilder.UseSqlite("Data Source=TimeTracker.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optionally, configure relationships and constraints here.
            // For example:
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Projects)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.TaskItems)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.TimeEntries)
                .WithOne(te => te.TaskItem)
                .HasForeignKey(te => te.TaskItemId);
        }
    }
}
