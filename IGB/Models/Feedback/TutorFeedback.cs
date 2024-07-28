using IGB.Models.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Feedback
{
    public class TutorFeedback
    {
        [Key]
        public long Id { get; set; }
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public long? LessonBookingId { get; set; }

        public bool PreviousHomeWorkDone { get; set; } = false;
        public double? PreviousHomeWorkDoneScore { get; set; } = 0; //----------------------

        public bool PreviousHomeWorkDiscussed { get; set; } = false;

        public string? TopicCoveredToday { get; set; }

        public double? TopicUnderstandingLevel { get; set; } = 0;// Dropdown (1-10) //----------------------

        public string? GradePrediction { get; set; }
        public double? GradePredictionScore { get; set; } = 0; //----------------------

        public string? AverageGradePrediction { get; set; } // Auto

        public double? MentalSkills { get; set; } = 0; // Dropdown (1-10) //---------------------

        public string? TestName { get; set; }
        public double? ObtainedScore { get; set; }
        public double? TotalScore { get; set; }

        public double? Percentage { get; set; }
        public double? PercentageScore { get; set; } = 0; //----------------------

        public byte[]? TestFile { get; set; }

        public byte[]? NextHomeworkFile { get; set; }
        public string? NextHomework { get; set; } // Given , Not Given
        public string? Remarks { get; set; }

        public string? Announcement { get; set; }

        public string? Status { get; set; }

        public bool IsPending { get; set; } = false;
        public bool IsApproved { get; set; } = false;

        public double? FinalScore { get; set; } = 0;


        [ForeignKey("LessonBookingId")]
        public virtual LessonBooking? LessonBookings { get; set; }


    }
}
