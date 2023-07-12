using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.WpfApp.Models.Csv;

public class CsvFileReader
{

    public ICollection<EmployeeDto> ReadAllEmployee(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<EmployeeMap>();
            var records = csv.GetRecords<EmployeeDto>();

            return records.ToArray();
        }
    }
}