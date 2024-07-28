using System.ComponentModel.DataAnnotations;
using FluentValidation;
using IGB.Models.Tutor;
using IGB.Models.Users;

namespace IGB.Models.Admin
{
    public class Curriculum
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }

        public virtual List<Course> Courses { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
        public virtual List<TutorSpeciality> TutorSpecialitys { get; set; }
        public virtual List<TutorExperience> TutorExperiences { get; set; }




        public class CurriculumValidator : AbstractValidator<Curriculum>
        {
            public CurriculumValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Curriculum Name Is Required");

            }
        }
    }
}
