using FluentValidation;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Tutor
{
    public class TutorEducation
    {
        [Key]
        public long Id { get; set; }
        public string? TutorId { get; set; }
        public string? UniversityLevel { get; set; }
        public string? IntermediateLevel { get; set; }
        public string? OtherIntermediateLevel { get; set; }
        public string? MatriculationLevel { get; set; }
        public string? OtherMatriculationLevel { get; set; }
        public string? Faculty { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }


        [ForeignKey("TutorId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }


        public class TutorEducationValidator : AbstractValidator<TutorEducation>
        {
            public TutorEducationValidator()
            {
                RuleFor(x => x.UniversityLevel).NotNull().NotEmpty().WithMessage("University Level Is Required");
                RuleFor(x => x.Faculty).NotNull().NotEmpty().WithMessage("Faculty Is Required");
                RuleFor(x => x.StartYear).NotNull().NotEmpty().WithMessage("StartYear Is Required");
                RuleFor(x => x.EndYear).NotNull().NotEmpty().WithMessage("EndYear Is Required");
     

            }
        }

    }
}
