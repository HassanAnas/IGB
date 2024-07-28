using IGB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.DTOs
{
    public class TutorFeedbackDto
    {
        public long? Id { get; set; }

        public long? LessonBookingId { get; set; }

        public bool PreviousHomeWorkDone { get; set; } = false;
        public bool PreviousHomeWorkDiscussed { get; set; } = false;
        public string? TopicCoveredToday { get; set; }
        public double? TopicUnderstandingLevel { get; set; } = 0;
        public string? GradePrediction { get; set; }
        public string? AverageGradePrediction { get; set; }
        public double? MentalSkills { get; set; } = 0;

        public string? TestName { get; set; }
        public double? TotalScore { get; set; }
        public double? ObtainedScore { get; set; }
        public double? Percentage { get; set; }

        public byte[]? TestFile { get; set; }
        public byte[]? NextHomeworkFile { get; set; }

        public string? NextHomework { get; set; } // Given , Not Given
        public string? Remarks { get; set; }
        public string? Announcement { get; set; }

    }
}
