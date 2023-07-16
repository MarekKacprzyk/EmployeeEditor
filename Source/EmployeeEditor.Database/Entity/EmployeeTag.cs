namespace EmployeeEditor.Database.Entity
{
    public record EmployeeTag
    {
        public int StudentId { get; set; }
        public EmployeeEntity Employee{ get; set; }

        public int CourseId { get; set; }
        public TagEntity Tags{ get; set; }
    }
}
