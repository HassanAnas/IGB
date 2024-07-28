
using FluentValidation;
using IGB.Models.Admin;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Tutor
{
    public class TutorSpeciality
    {
        [Key]
        public long Id { get; set; }
        public string? TutorId { get; set; }
        public long? CurriculumId { get; set; }
        public string? CurriculumName { get; set; }
        public long? GradeId { get; set; }
        public string? GradeName { get; set; }
        public long? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? ExpLevel { get; set; }

        [ForeignKey("TutorId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }
        [ForeignKey("CurriculumId")]
        public virtual Curriculum? Curriculums { get; set; }
        [ForeignKey("GradeId")]
        public virtual Grade? Grades { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Courses { get; set; }


        public class TutorSpecialityValidator : AbstractValidator<TutorSpeciality>
        {
            public TutorSpecialityValidator()
            {
                RuleFor(x => x.CurriculumId).NotNull().NotEmpty().WithMessage("Curriculum Is Required");
                RuleFor(x => x.GradeId).NotNull().NotEmpty().WithMessage("Grade Is Required");
                RuleFor(x => x.CourseId).NotNull().NotEmpty().WithMessage("Course Is Required");
                RuleFor(x => x.ExpLevel).NotNull().NotEmpty().WithMessage("Expertise Level Is Required");

            }
        }
    }
}
