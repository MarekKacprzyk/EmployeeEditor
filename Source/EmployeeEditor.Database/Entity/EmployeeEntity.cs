using System.ComponentModel.DataAnnotations;

namespace EmployeeEditor.Database.Entity
{
    public record EmployeeEntity
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
