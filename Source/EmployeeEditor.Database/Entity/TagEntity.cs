using System.ComponentModel.DataAnnotations;

namespace EmployeeEditor.Database.Entity
{
    public record TagEntity
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public string Name { get; init; }
        [Required]
        public string Description { get; init; }

        public virtual List<EmployeeEntity> Employees { get; init; } = new List<EmployeeEntity>();
    }
}
