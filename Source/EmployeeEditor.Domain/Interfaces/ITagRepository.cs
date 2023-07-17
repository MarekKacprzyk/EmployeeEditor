using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.Domain.Interfaces;

public interface ITagRepository
{
    Task<TagDto> AddTag(TagDto tag, EmployeeDto employeeDto);
    Task<TagDto> UpdateTag(TagDto tag);
    Task DeleteTag(TagDto tag, EmployeeDto employeeDto);

    Task<IQueryable<TagDto>> GetTagForUser(EmployeeDto employee);
}