using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmployeeEditor.Database
{
    public class EmployeeDbContextFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
    {
        public EmployeeDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptions<EmployeeDbContext>();
            var dbContext = new EmployeeDbContext(options);
            dbContext.Database.Migrate();
            return dbContext;
        }
    }
}
