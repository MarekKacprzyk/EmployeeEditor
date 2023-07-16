using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.Domain.Interfaces;

public interface ITagRepository
{
    Task AddTag(TagDto tag);
    Task UpdateTag(TagDto tag);
    Task DeleteTag(TagDto tag);

    Task<IQueryable<TagDto>> GetTagForUser(EmployeeDto employee);
}