namespace IGB.DTOs
{
    public class TutorExperienceDto
    {
        public long Id { get; set; }
        public string? InstituteName { get; set; }
        public string? Designation { get; set; }
        public long? CurriculumId { get; set; }
        public string? CurriculumName { get; set; }
        public long? CourseId { get; set; }
        public string? CourseName { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
