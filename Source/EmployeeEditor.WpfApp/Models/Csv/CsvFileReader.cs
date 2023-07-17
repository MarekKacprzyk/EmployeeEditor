using System;
using CsvHelper;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.Domain.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using EmployeeEditor.WpfApp.Models.Validators;

namespace EmployeeEditor.WpfApp.Models.Csv;

public class CsvFileReader : IAppService
{
    private readonly EmployeeValidator _validator;

    public CsvFileReader(EmployeeValidator validator)
    {
        _validator = validator;
    }

    public ICollection<EmployeeDto> ReadAllEmployee(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<EmployeeMap>();
            var records = csv.GetRecords<EmployeeDto>().ToArray();

            var invalidEmployess = records.Where(r => !_validator.Validate(r).IsValid).ToArray();

            if (!invalidEmployess.Any()) return records.ToArray();

            var builder = new StringBuilder();

            foreach (var employee in invalidEmployess)
            {
                builder.AppendLine("Pracowni:");
                builder.AppendLine($"\tId: {employee.Id} ");
                builder.AppendLine($"\tName: {employee.Name} ");
                builder.AppendLine($"\tSurename: {employee.Surename} ");
                builder.AppendLine($"\temail: {employee.Email} ");
                builder.AppendLine($"\tphone: {employee.PhoneNumber} ");
            }

            throw new ArgumentException($"Nieprawidłowe dane dla Pracowników:\n\n{builder}");
        }
    }
}