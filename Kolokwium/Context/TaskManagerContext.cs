using Kolokwium.Models;
using Microsoft.EntityFrameworkCore;
using Task = Kolokwium.Models.Task;

namespace Kolokwium.Context
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany()
                .HasForeignKey(t => t.IdProject);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Reporter)
                .WithMany()
                .HasForeignKey(t => t.IdReporter)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.IdAssignee)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.DefaultAssignee)
                .WithMany()
                .HasForeignKey(p => p.IdDefaultAssignee)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Access>()
                .HasKey(a => new { a.IdUser, a.IdProject });

            modelBuilder.Entity<Access>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.IdUser);

            modelBuilder.Entity<Access>()
                .HasOne(a => a.Project)
                .WithMany()
                .HasForeignKey(a => a.IdProject);
        }
    }
}
