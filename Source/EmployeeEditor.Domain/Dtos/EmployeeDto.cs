using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEditor.Domain.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public string Surename { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
