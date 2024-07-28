
using FluentValidation;
using IGB.Models.Admin;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Tutor
{
    public class TutorExperience
    {
        [Key]
        public long Id { get; set; }
        public string? TutorId { get; set; }
        public string? InstituteName { get; set; }
        public string? Designation { get; set; }
        public long? CurriculumId { get; set; }
        public string? CurriculumName { get; set; }
        public long? CourseId { get; set; }
        public string? CourseName { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public bool IsEndYear { get; set; } = false;


        [ForeignKey("TutorId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }
        [ForeignKey("CurriculumId")]
        public virtual Curriculum? Curriculums { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Courses { get; set; }

       


        public class TutorExperienceValidator : AbstractValidator<TutorExperience>
        {
            public TutorExperienceValidator()
            {
                RuleFor(x => x.InstituteName).NotNull().NotEmpty().WithMessage("Institute Name Is Required");
                RuleFor(x => x.Designation).NotNull().NotEmpty().WithMessage("Designation Is Required");
                RuleFor(x => x.CurriculumId).NotNull().NotEmpty().WithMessage("Curriculum Is Required");
                RuleFor(x => x.CourseId).NotNull().NotEmpty().WithMessage("Course Is Required");
                RuleFor(x => x.Designation).NotNull().NotEmpty().WithMessage("Designation Is Required");
                RuleFor(x => x.StartYear).NotNull().NotEmpty().WithMessage("Start Year Is Required");
                RuleFor(x => x.EndYear).NotNull().NotEmpty().WithMessage("End Year Is Required");          

            }
        }
    }
}
