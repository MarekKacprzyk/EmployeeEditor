using AutoMapper;
using EmployeeEditor.Database.Entity;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeEditor.Database.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task SetNewEmployeesCollection(params EmployeeDto[] employees)
        {
            var existingEmployees = _dbContext.Employees.ToList();
            _dbContext.Employees.RemoveRange(existingEmployees);

            var personsToAdd = _mapper.Map<EmployeeEntity[]>(employees);
            await _dbContext.Employees.AddRangeAsync(personsToAdd);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployee(EmployeeDto employee)
        {
            var existingPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (existingPerson is null)
            {
                throw new EntryPointNotFoundException("Employee not found");
            }

            _dbContext.Employees.Remove(existingPerson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployee(EmployeeDto employee)
        {
            var existingPerson = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (existingPerson is null)
            {
                throw new EntryPointNotFoundException("Employee not found");
            }

            _mapper.Map(existingPerson, existingPerson);

            await _dbContext.SaveChangesAsync();
        }
    }
}