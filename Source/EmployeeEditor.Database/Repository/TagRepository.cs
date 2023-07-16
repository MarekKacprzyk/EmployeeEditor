using AutoMapper;
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
    public Task AddTag(TagDto tag)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTag(TagDto tag)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTag(TagDto tag)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<TagDto>> GetTagForUser(EmployeeDto employee)
    {
        var tags = _dbContext.Tags.Where(t => t.Employees.Any(e => e.Id == employee.Id));
        //var hh = _mapper.ProjectTo(tags);
        return null;
    }
}