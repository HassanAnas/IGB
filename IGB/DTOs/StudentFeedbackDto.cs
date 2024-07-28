namespace IGB.DTOs
{
    public class StudentFeedbackDto
    {
        public long? Id { get; set; }
        public long? LessonBookingId { get; set; }
        public string? UnderstandingAndEngagement { get; set; }
        public string? DidTutorExplainTopic { get; set; }
        public string? DidTutorClearDoubtToday { get; set; }
        public string? DidTutorClearDoubtPrevious { get; set; }
        public string? RateTutorTeaching { get; set; }
        public string? Remarks { get; set; }
    }
}
