using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeEntity = EmployeeEditor.Database.Entity.EmployeeEntity;

namespace EmployeeEditor.Database
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<EmployeeEntity> Employee { get; set; }

        public EmployeeDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Employee_Database.sqlite");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
