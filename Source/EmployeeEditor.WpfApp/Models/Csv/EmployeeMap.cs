using CsvHelper.Configuration;
using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.WpfApp.Models.Csv;

public class EmployeeMap : ClassMap<EmployeeDto>
{
    public EmployeeMap()
    {
        Map(m => m.Id).Name("id");
        Map(m => m.Name).Name("name");
        Map(m => m.Surename).Name("surename");
        Map(m => m.Email).Name("email");
        Map(m => m.PhoneNumber).Name("phone");
    }
}