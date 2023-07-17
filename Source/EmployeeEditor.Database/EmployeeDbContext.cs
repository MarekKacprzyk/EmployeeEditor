using EmployeeEditor.Database.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EmployeeEntity = EmployeeEditor.Database.Entity.EmployeeEntity;

namespace EmployeeEditor.Database
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

        public EmployeeDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"database{DateTime.Now:ddHHmmss}.sqlite";
            var fullPath = Path.Combine(myDocPath, fileName);

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={fullPath}")
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.NonQueryOperationFailed));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeEntity>()
                .HasMany(s => s.Tags)
                .WithMany(c => c.Employees)
                .UsingEntity<EmployeeTag>(
                    j => j.HasOne(sc => sc.Tags).WithMany(),
                    j => j.HasOne(sc => sc.Employee).WithMany()
                );
        }
    }
}
