using FluentValidation;
using IGB.Models.Admin;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models
{
    public class CourseBooking
    {
        [Key]
        public long Id { get; set; }
        public DateTime? CourseBookingDate { get; set; } = DateTime.UtcNow;
        public string? StudentId { get; set; }
        public long? CourseId { get; set; }
        public string? TutorId { get; set; }
        public string? AdminId { get; set; } = null;
        public string? StudentRemarks { get; set; }
        public string? TutorRemarks { get; set; } = null;
        public string? AdminRemarks { get; set; } = null;
        public string? Status { get; set; }


        public bool IsSubmited { get; set; } = false;

        public bool IsAdminApproved { get; set; } = false;
        public bool IsAdminRejected { get; set; } = false;
        public bool IsTutorApproved { get; set; } = false;
        public bool IsTutorRejected { get; set; } = false;

        public bool IsBooked { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

        public bool Smooth { get; set; } = true;
        public bool Marker { get; set; } = true;
        public bool Detail { get; set; } = true;



        [ForeignKey("StudentId")]
        public virtual ApplicationUser? StudentApplicationUsers { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Courses { get; set; }
        [ForeignKey("TutorId")]
        public virtual ApplicationUser? TutorApplicationUsers { get; set; }
        [ForeignKey("AdminId")]
        public virtual ApplicationUser? AdminApplicationUsers { get; set; }




        public virtual List<StudentCredit> StudentCredits { get; set; }
        public virtual List<LessonBooking> LessonBookings { get; set; }

        public class CourseBookingValidator : AbstractValidator<CourseBooking>
        {
            public CourseBookingValidator()
            {
                RuleFor(x => x.CourseId).NotNull().NotEmpty().WithMessage("Course Is Required");

            }
        }

        public class CourseApproveValidator : AbstractValidator<CourseBooking>
        {
            public CourseApproveValidator()
            {
                RuleFor(x => x.TutorId).NotNull().NotEmpty().WithMessage("Tutor Is Required");
            }
        }
    }
}
