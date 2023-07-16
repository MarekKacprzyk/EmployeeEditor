using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task SetNewEmployeesCollection(params EmployeeDto[] employees);
        Task DeleteEmployee(EmployeeDto employee);
        Task UpdateEmployee(EmployeeDto employee);
    }
}
