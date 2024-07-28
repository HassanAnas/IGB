namespace IGB.DTOs
{
    public class TutorEducationDto
    {
        public long Id { get; set; }
        public string? UniversityLevel { get; set; }
        public string? IntermediateLevel { get; set; }
        public string? OtherIntermediateLevel { get; set; }
        public string? MatriculationLevel { get; set; }
        public string? OtherMatriculationLevel { get; set; }
        public string? Faculty { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
