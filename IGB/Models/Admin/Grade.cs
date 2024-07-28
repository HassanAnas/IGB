using FluentValidation;
using IGB.Models.Tutor;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace IGB.Models.Admin
{
    public class Grade
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }

        public virtual List<GradeCourse> GradeCourses { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
        public virtual List<TutorSpeciality> TutorSpecialitys { get; set; }






        public class GradeValidator : AbstractValidator<Grade>
        {
            public GradeValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Grade Name Is Required");

            }
        }
    }
}
