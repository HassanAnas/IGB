using IGB.Models.Feedback;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models
{
    public class AllNotification
    {
        [Key]
        public long Id { get; set; }

        public string? NewUserId { get; set; }
        public string? NewUserRole { get; set; }

        public long? CourseBookingId { get; set; }

        public long? CourseBookingIdForLesson { get; set; }

        public long? LessonBookingId { get; set; }

        public long? LessonBookingStartId { get; set; }

        public long? TutorFeedbackId { get; set; }

        public long? StudentFeedbackId { get; set; }

        public bool ForAdmin { get; set; } = false;
        public bool ForTutor { get; set; } = false;
        public bool ForStudent { get; set; } = false;
        public string? ForTutorId { get; set; } = null;
        public string? ForStudentId { get; set; } = null;

        public bool IsReadByAdmin { get; set; } = false;
        public bool IsReadByTutor { get; set; } = false;
        public bool IsReadByStudent { get; set; } = false;

        public bool IsApprovedByAdmin { get; set; } = false;
        public bool IsApprovedByTutor { get; set; } = false;
        public bool IsApprovedByStudent { get; set; } = false;

        public bool IsRejectedByAdmin { get; set; } = false;
        public bool IsRejectedByTutor { get; set; } = false;
        public bool IsRejectedByStudent { get; set; } = false;

        public string? Notification { get; set; }




        public DateTime? Time { get; set; } = DateTime.UtcNow;



        [ForeignKey("NewUserId")]
        public virtual ApplicationUser? NewApplicationUsers { get; set; }

        [ForeignKey("CourseBookingId")]
        public virtual CourseBooking? CourseBookings { get; set; }

        [ForeignKey("CourseBookingIdForLesson")]
        public virtual CourseBooking? CourseBookingIdForLessons { get; set; }

        [ForeignKey("LessonBookingId")]
        public virtual LessonBooking? LessonBookings { get; set; }

        [ForeignKey("LessonBookingStartId")]
        public virtual LessonBooking? LessonBookingStarts { get; set; }

        [ForeignKey("ForTutorId")]
        public virtual ApplicationUser? TutorApplicationUsers { get; set; }
        [ForeignKey("ForStudentId")]
        public virtual ApplicationUser? StudentApplicationUsers { get; set; }

        [ForeignKey("TutorFeedbackId")]
        public virtual TutorFeedback? TutorFeedbacks { get; set; }

        [ForeignKey("StudentFeedbackId")]
        public virtual StudentFeedback? StudentFeedbacks { get; set; }
    }
}
