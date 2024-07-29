namespace IGB.DTOs
{
    public class LessonBookingUpdateDto
    {
        public long? CourseBookingId { get; set; }
        public string? LessonBookingUpdatedBy { get; set; }
        public string? LessonName { get; set; }
        public string? TopicCovered { get; set; }
        public long? FirstTimeSlot { get; set; }
        public long? SecondTimeSlot { get; set; }
        public long? ThirdTimeSlot { get; set; }
        public int? Duration { get; set; }
        public string? StudentRemarks { get; set; }
        public string? TutorRemarks { get; set; }
    }
}
