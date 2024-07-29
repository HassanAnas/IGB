namespace IGB.DTOs
{
    public class LessonBookingCreateListDto
    {
        public long? CourseBookingId { get; set; }
        public string? LessonBookingInitiatedBy { get; set; }
        public List<LessonBookingCreateDto> LessonBookingCreateDtos { get; set; }
    }
}
