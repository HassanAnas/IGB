using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Feedback
{
    public class StudentFeedback
    {
        [Key]
        public long Id { get; set; }
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public long? LessonBookingId { get; set; }

        public string? UnderstandingAndEngagement { get; set; }
        public double? UnderstandingAndEngagementScore { get; set; }

        public string? DidTutorExplainTopic { get; set; }
        public double? DidTutorExplainTopicScore { get; set; }

        public string? DidTutorClearDoubtToday { get; set; }
        public double? DidTutorClearDoubtTodayScore { get; set; }

        public string? DidTutorClearDoubtPrevious { get; set; }
        public double? DidTutorClearDoubtPreviousScore { get; set; }

        public string? RateTutorTeaching { get; set; }
        public double? RateTutorTeachingScore { get; set; }

        public double? FinalScore { get; set; }
        public string? Remarks { get; set; }

        public string? Status { get; set; }

        public bool IsPending { get; set; }=false;
        public bool IsApproved { get; set; }=false;




        [ForeignKey("LessonBookingId")]
        public virtual LessonBooking? LessonBookings { get; set; }
    }
}
