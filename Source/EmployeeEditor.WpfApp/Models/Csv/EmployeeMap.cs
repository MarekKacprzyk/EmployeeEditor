using CsvHelper.Configuration;
using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.WpfApp.Models.Csv;

public class EmployeeMap : ClassMap<EmployeeDto>
{
    public EmployeeMap()
    {
        Map(m => m.Id).Name("id");
        Map(m => m.Name).Name("name");
    }
}