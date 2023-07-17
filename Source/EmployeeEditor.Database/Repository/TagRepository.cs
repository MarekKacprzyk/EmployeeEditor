using AutoMapper;
using EmployeeEditor.Database.Entity;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.Domain.Interfaces;

namespace EmployeeEditor.Database.Repository;

public class TagRepository : ITagRepository
{
    private readonly EmployeeDbContext _dbContext;
    private readonly IMapper _mapper;

    public TagRepository(EmployeeDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<TagDto> AddTag(TagDto tag, EmployeeDto employee)
    {
        var tagEntity = _mapper.Map<TagEntity>(tag);
        if (tagEntity is null)
        {
            throw new InvalidDataException($"tag o ID: {tag.Id} już istnieje");
        }

        _dbContext.Tags.Add(tagEntity);
        var employeeEntity = _dbContext.Employees.FirstOrDefault(e => e.Id == employee.Id);
        employeeEntity?.Tags.Add(tagEntity);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<TagDto>(tagEntity);
    }

    public Task<TagDto> UpdateTag(TagDto tag)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTag(TagDto tag, EmployeeDto employeeDto)
    {
        throw new NotImplementedException();
    }

    public async Task<IQueryable<TagDto>> GetTagForUser(EmployeeDto employee)
    {
        var tags = _dbContext.Tags.Where(t => t.Employees.Any(e => e.Id == employee.Id));

        if (tags is null)
        {
            throw new EntryPointNotFoundException(
                $"Nie znaleziono tagów dla użytkownika {employee.Name} {employee.Surename}");
        }

        var tagDtos = _mapper.Map<IQueryable<TagDto>>(tags);
        return tagDtos;
    }
}