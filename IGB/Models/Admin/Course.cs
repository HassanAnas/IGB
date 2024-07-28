using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using IGB.Models.Tutor;

namespace IGB.Models.Admin
{
    public class Course

    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? CurriculumId { get; set; }

        [ForeignKey("CurriculumId")]
        public virtual Curriculum? Curriculums { get; set; }

        public virtual List<GradeCourse> GradeCourses { get; set; }
        public virtual List<CourseTopic> CourseTopics { get; set; }
        public virtual List<TutorSpeciality> TutorSpecialitys { get; set; }
        public virtual List<TutorExperience> TutorExperience { get; set; }



        public class CourseValidator : AbstractValidator<Course>
        {
            public CourseValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Course Name Is Required");
                RuleFor(x => x.CurriculumId).NotNull().NotEmpty().WithMessage("Curriculum Name Is Required");

            }
        }
    }
}
