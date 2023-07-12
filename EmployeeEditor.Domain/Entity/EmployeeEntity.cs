using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEditor.Domain.Entity
{
    internal record EmployeeEntity
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        public string Surename { get; init; }
        
        [Required]
        public string Email { get; init; }
        
        public string PhoneNumber { get; init; }
    }
}
