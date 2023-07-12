using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEditor.Domain.Dtos
{
    internal class EmployeeDto
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Surename { get; init; }

        public string Email { get; init; }

        public string PhoneNumber { get; init; }
    }
}
