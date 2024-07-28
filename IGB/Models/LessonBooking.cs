using FluentValidation;
using IGB.Models.Feedback;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models
{
    public class LessonBooking
    {
        [Key]
        public long Id { get; set; }
        public DateTime? LessonBookingDate { get; set; } = DateTime.UtcNow;
        public long? CourseBookingId { get; set; }
        public string? LessonBookingInitiatedBy { get; set; }

        public string? LessonName { get; set; }
        public string? TopicCovered { get; set; }
        public DateTime? FirstTimeSlot { get; set; }
        public DateTime? SecondTimeSlot { get; set; }
        public DateTime? ThirdTimeSlot { get; set; }
        public int? Duration { get; set; }

        public DateTime? SelectedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string? Status { get; set; }
        public string? StudentRemarks { get; set; }
        public string? TutorRemarks { get; set; }
        public string? AdminRemarks { get; set; }

        public string? ClassLink { get; set; }
        public bool TutorClassStartAccess { get; set; } = false;
        public bool StudentClassStartAccess { get; set; } = false;




        public bool IsSubmited { get; set; } = false;

        public bool IsAdminApproved { get; set; } = false;
        public bool IsAdminRejected { get; set; } = false;

        public bool IsTutorApproved { get; set; } = false;
        public bool IsTutorRejected { get; set; } = false;

        public bool IsStudentApproved { get; set; } = false;
        public bool IsStudentRejected { get; set; } = false;

        public bool IsBooked { get; set; } = false;
        public bool IsDone { get; set; } = false;
        public bool IsCancelled { get; set; } = false;


        //---------------------------

        public bool IsSessionInitiated { get; set; } = false;
        public bool IsSessionEnded { get; set; } = false;
        public string? SessionInitiatedBy { get; set; }
        public string? SessionEndeddBy { get; set; }
        public TimeSpan? SessionTime { get; set; }
        public TimeSpan? SessionTutorTime { get; set; }
        public TimeSpan? SessionStudentTime { get; set; }

        public bool IsStudentPresent { get; set; } = false;
        public DateTime? StudentComingTime { get; set; }
        public DateTime? StudentCheckoutTime { get; set; }

        public bool IsTutorPresent { get; set; } = false;
        public DateTime? TutorComingTime { get; set; }
        public DateTime? TutorCheckoutTime { get; set; }

        public double? CreditCharged { get; set; } = 0;
        public double? EstimatedCreditCharged { get; set; } = 0;


        public double? StudentScore { get; set; } = 0;
        public double? TutorScore { get; set; } = 0;





        [ForeignKey("CourseBookingId")]
        public virtual CourseBooking? CourseBookings { get; set; }


        public virtual TutorFeedback? TutorFeedbacks { get; set; }
        public virtual StudentFeedback? StudentFeedbacks { get; set; }




   


        public class LessonBookingValidator : AbstractValidator<LessonBooking>
        {
            public LessonBookingValidator()
            {
                RuleFor(x => x.LessonName).NotNull().NotEmpty().WithMessage("Lesson Name Is Required");
                RuleFor(x => x.FirstTimeSlot)
                    .NotNull().NotEmpty().WithMessage("First Time Slot is required")
                    .Must(BeInTheFuture).WithMessage("First Time Slot must be in the future");

                RuleFor(x => x.SecondTimeSlot)
                   .NotNull().NotEmpty().WithMessage("Second Time Slot is required")
                   .Must(BeInTheFuture).WithMessage("Second Time Slot must be in the future");

                RuleFor(x => x.ThirdTimeSlot)
                   .NotNull().NotEmpty().WithMessage("Third Time Slot is required")
                   .Must(BeInTheFuture).WithMessage("Third Time Slot must be in the future");

                RuleFor(x => x.Duration).NotNull().NotEmpty().WithMessage("Lesson  Is Required");

                RuleFor(x => x).Custom((booking, context) =>
                {
                    if (booking.FirstTimeSlot.HasValue && booking.SecondTimeSlot.HasValue)
                    {
                        if (booking.FirstTimeSlot.Value == booking.SecondTimeSlot.Value)
                        {
                            context.AddFailure("SecondTimeSlot", "Second Time Slot must be different from First Time Slot");
                        }
                    }

                    if (booking.FirstTimeSlot.HasValue && booking.ThirdTimeSlot.HasValue)
                    {
                        if (booking.FirstTimeSlot.Value == booking.ThirdTimeSlot.Value)
                        {
                            context.AddFailure("ThirdTimeSlot", "Third Time Slot must be different from First Time Slot");
                        }
                    }

                    if (booking.SecondTimeSlot.HasValue && booking.ThirdTimeSlot.HasValue)
                    {
                        if (booking.SecondTimeSlot.Value == booking.ThirdTimeSlot.Value)
                        {
                            context.AddFailure("ThirdTimeSlot", "Third Time Slot must be different from Second Time Slot");
                        }
                    }
                });
            }
            private bool BeInTheFuture(DateTime? timeSlot)
            {
                return timeSlot.HasValue && timeSlot.Value > DateTime.Now;
            }
        }
    }
}
