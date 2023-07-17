using AutoMapper;
using EmployeeEditor.Database.Entity;
using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.Database.Map;

public class MappingTag : Profile
{
    public MappingTag()
    {
        CreateMap<TagDto, TagEntity>();
        CreateMap<TagEntity, TagDto>();
    }
}