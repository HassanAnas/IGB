using FluentValidation;
using IGB.Models.Admin;
using IGB.Models.Tutor;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using IGB.Data;
using Microsoft.EntityFrameworkCore;

namespace IGB.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string? Tag { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SchoolName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? ResidingCountry { get; set; }
        public string? TimeZone { get; set; }
        public string? LocalNumber { get; set; }
        public string? WhatsappNumber { get; set; }
        public long? CurriculumId { get; set; }
        public long? GradeId { get; set; }
        public string? HomeAddress { get; set; }
        public byte[]? Image { get; set; }


        public string? StudentId { get; set; }
        public string? RelationshipWithStudent { get; set; }


        public string? RegularIrregular { get; set; }
        public bool ExtraCaring { get; set; } = false;


        public string? ProfileLink { get; set; }
        public string? EmployeeType { get; set; }
        public string? OtherEmployeeType { get; set; }

        public byte[]? Contract { get; set; }



        public bool IsSuperAdmin { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsTutor { get; set; } = false;
        public bool IsStudent { get; set; } = false;
        public bool IsFirstGuardian { get; set; } = false;
        public bool IsSecondGuardian { get; set; } = false;

        public string? RoleName { get; set; }
        public bool IsProfileUpdated { get; set; } = false;
        public bool IsFirstGuardianUpdated { get; set; } = false;
        public bool IsSecondGuardianUpdated { get; set; } = false;

        public bool IsSpecialiitiesUpdated { get; set; } = false;
        public bool IsEducationUpdated { get; set; } = false;
        public bool IsExperienceUpdated { get; set; } = false;
        public bool IsDocumentUpdated { get; set; } = false;
        public bool IsContracttUpdated { get; set; } = false;

        public bool IsUpdated { get; set; } = false;
        public string? Update { get; set; } = "Not Updated";
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = false;
 

        [ForeignKey("CurriculumId")]
        public virtual Admin.Curriculum? Curriculums { get; set; }
        [ForeignKey("GradeId")]
        public virtual Grade? Grades { get; set; }
        [ForeignKey("StudentId")]
        public virtual ApplicationUser? StudentApplicationUsers { get; set; }

        public virtual List<TutorSpeciality> TutorSpecialitys { get; set; }
        public virtual List<TutorEducation> TutorEducations { get; set; }
        public virtual List<TutorExperience> TutorExperiences { get; set; }
        public virtual List<TutorDocument> TutorDocuments { get; set; }
        public virtual List<AdminDocument> AdminDocuments { get; set; }
        public virtual List<StudentCredit> StudentCredits { get; set; }

        public class UserCreateValidator : AbstractValidator<ApplicationUser>
        {
            private readonly ApplicationDbContext _context;

            public UserCreateValidator(ApplicationDbContext context)
            {
                _context = context;

                RuleFor(x => x.Email)
                    .NotNull().NotEmpty().WithMessage("Email Is Required")
                    .EmailAddress().WithMessage("Email Is Not Valid")
                    .Must(EmailNotExists).WithMessage("Email already exists");

                RuleFor(x => x.PasswordHash)
                    .NotEmpty().NotNull().WithMessage("Password is required")
                    .Must(ContainUpperAndLower).WithMessage("Password must contain both upper and lowercase characters")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            }

            private bool ContainUpperAndLower(string password)
            {
                return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper) && password.Any(char.IsLower);
            }

            private bool EmailNotExists(string email)
            {
                return !_context.Users.Any(u => u.Email == email);
            }
        }



        public class StudentProfileValidator : AbstractValidator<ApplicationUser>
        {
            public StudentProfileValidator()
            {
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First Name Is Required");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last Name Is Required");
                RuleFor(x => x.DateOfBirth).NotNull().NotEmpty().WithMessage("Date Of Birth Is Required");
                RuleFor(x => x.Gender).NotNull().NotEmpty().WithMessage("Gender Is Required");
                RuleFor(x => x.Nationality).NotNull().NotEmpty().WithMessage("Nationalty Is Required");
                RuleFor(x => x.ResidingCountry).NotNull().NotEmpty().WithMessage("Residing Country Is Required");
                RuleFor(x => x.TimeZone).NotNull().NotEmpty().WithMessage("TimeZone Is Required");
                RuleFor(x => x.LocalNumber).NotNull().NotEmpty().WithMessage("Local Number Is Required");
                RuleFor(x => x.WhatsappNumber).NotNull().NotEmpty().WithMessage("Whatsapp Number Is Required");
                RuleFor(x => x.CurriculumId).NotNull().NotEmpty().WithMessage("Curriculum Is Required");
                RuleFor(x => x.GradeId).NotNull().NotEmpty().WithMessage("Grade Is Required");
                RuleFor(x => x.HomeAddress).NotNull().NotEmpty().WithMessage("Home Address Is Required");
                RuleFor(x => x.Image).NotNull().NotEmpty().WithMessage("Profile Picture Is Required");
                RuleFor(x => x.Email)
         .NotNull().NotEmpty().WithMessage("Email Is Required")
         .EmailAddress().WithMessage("Email Is Not Valid");
                RuleFor(x => x.PasswordHash)
                   .NotEmpty().NotNull().WithMessage("Password is required")
                   .Must(ContainUpperAndLower).WithMessage("Password must contain both upper and lowercase characters")
                   .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            }
            private bool ContainUpperAndLower(string password)
            {
                return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper) && password.Any(char.IsLower);
            }
        }


        public class FirstGuardianValidator : AbstractValidator<ApplicationUser>
        {
            private readonly ApplicationDbContext _context;
            public FirstGuardianValidator(ApplicationDbContext context)
            {
                _context = context;

                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Guardian Name Is Required");
                RuleFor(x => x.RelationshipWithStudent).NotNull().NotEmpty().WithMessage("Relationship With Student Is Required");
                RuleFor(x => x.LocalNumber).NotNull().NotEmpty().WithMessage("Local Number Is Required");
                RuleFor(x => x.WhatsappNumber).NotNull().NotEmpty().WithMessage("Whatsapp Number Is Required");
                RuleFor(x => x.HomeAddress).NotNull().NotEmpty().WithMessage("HomeAddress Is Required");
                RuleFor(x => x.Email)
      .NotNull().NotEmpty().WithMessage("Email Is Required")
      .EmailAddress().WithMessage("Email Is Not Valid")
      .Must(EmailNotExists).WithMessage("Email already exists");
                RuleFor(x => x.PasswordHash)
                   .NotEmpty().NotNull().WithMessage("Password is required")
                   .Must(ContainUpperAndLower).WithMessage("Password must contain both upper and lowercase characters")
                   .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            }
            private bool ContainUpperAndLower(string password)
            {
                return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper) && password.Any(char.IsLower);
            }
            private bool EmailNotExists(string email)
            {
                return !_context.Users.Any(u => u.Email == email);
            }
        }

        public class SecondGuardianValidator : AbstractValidator<ApplicationUser>
        {
            private readonly ApplicationDbContext _context;
            public SecondGuardianValidator(ApplicationDbContext context)
            {
                _context = context;

                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("Guardian Name Is Required");
                RuleFor(x => x.RelationshipWithStudent).NotNull().NotEmpty().WithMessage("Relationship With Student Is Required");
                RuleFor(x => x.LocalNumber).NotNull().NotEmpty().WithMessage("Local Number Is Required");
                RuleFor(x => x.WhatsappNumber).NotNull().NotEmpty().WithMessage("Whatsapp Number Is Required");
                RuleFor(x => x.HomeAddress).NotNull().NotEmpty().WithMessage("HomeAddress Is Required");
                RuleFor(x => x.Email)
       .NotNull().NotEmpty().WithMessage("Email Is Required")
       .EmailAddress().WithMessage("Email Is Not Valid")
       .Must(EmailNotExists).WithMessage("Email already exists");
                RuleFor(x => x.PasswordHash)
                   .NotEmpty().NotNull().WithMessage("Password is required")
                   .Must(ContainUpperAndLower).WithMessage("Password must contain both upper and lowercase characters")
                   .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            }
            private bool ContainUpperAndLower(string password)
            {
                return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper) && password.Any(char.IsLower);
            }
            private bool EmailNotExists(string email)
            {
                return !_context.Users.Any(u => u.Email == email);
            }
        }





        public class TutorProfileValidator : AbstractValidator<ApplicationUser>
        {
            public TutorProfileValidator()
            {
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First Name Is Required");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last Name Is Required");
                RuleFor(x => x.EmployeeType).NotNull().NotEmpty().WithMessage("Tutor Type Is Required");
                RuleFor(x => x.ProfileLink).NotNull().NotEmpty().WithMessage("Profile Linl Is Required");
                RuleFor(x => x.DateOfBirth).NotNull().NotEmpty().WithMessage("Date Of Birth Is Required");
                RuleFor(x => x.Gender).NotNull().NotEmpty().WithMessage("Gender Is Required");
                RuleFor(x => x.Nationality).NotNull().NotEmpty().WithMessage("Nationalty Is Required");
                RuleFor(x => x.ResidingCountry).NotNull().NotEmpty().WithMessage("Residing Country Is Required");
                RuleFor(x => x.TimeZone).NotNull().NotEmpty().WithMessage("TimeZone Is Required");
                RuleFor(x => x.LocalNumber).NotNull().NotEmpty().WithMessage("Local Number Is Required");
                RuleFor(x => x.WhatsappNumber).NotNull().NotEmpty().WithMessage("Whatsapp Number Is Required");
                RuleFor(x => x.HomeAddress).NotNull().NotEmpty().WithMessage("Home Address Is Required");
                RuleFor(x => x.Image).NotNull().NotEmpty().WithMessage("Profile Picture Is Required");
                RuleFor(x => x.Email)
         .NotNull().NotEmpty().WithMessage("Email Is Required")
         .EmailAddress().WithMessage("Email Is Not Valid");
                RuleFor(x => x.PasswordHash)
                   .NotEmpty().NotNull().WithMessage("Password is required")
                   .Must(ContainUpperAndLower).WithMessage("Password must contain both upper and lowercase characters")
                   .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            }
            private bool ContainUpperAndLower(string password)
            {
                return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper) && password.Any(char.IsLower);
            }
        }

        public class AdminProfileValidator : AbstractValidator<ApplicationUser>
        {
            public AdminProfileValidator()
            {
                RuleFor(x => x.FirstName).NotNull().NotEmpty().WithMessage("First Name Is Required");
                RuleFor(x => x.LastName).NotNull().NotEmpty().WithMessage("Last Name Is Required");
                RuleFor(x => x.EmployeeType).NotNull().NotEmpty().WithMessage("Tutor Type Is Required");
                RuleFor(x => x.DateOfBirth).NotNull().NotEmpty().WithMessage("Date Of Birth Is Required");
                RuleFor(x => x.Gender).NotNull().NotEmpty().WithMessage("Gender Is Required");
                RuleFor(x => x.Nationality).NotNull().NotEmpty().WithMessage("Nationalty Is Required");
                RuleFor(x => x.ResidingCountry).NotNull().NotEmpty().WithMessage("Residing Country Is Required");
                RuleFor(x => x.TimeZone).NotNull().NotEmpty().WithMessage("TimeZone Is Required");
                RuleFor(x => x.LocalNumber).NotNull().NotEmpty().WithMessage("Local Number Is Required");
                RuleFor(x => x.WhatsappNumber).NotNull().NotEmpty().WithMessage("Whatsapp Number Is Required");
                RuleFor(x => x.HomeAddress).NotNull().NotEmpty().WithMessage("Home Address Is Required");
                RuleFor(x => x.Image).NotNull().NotEmpty().WithMessage("Profile Picture Is Required");
                RuleFor(x => x.Email)
         .NotNull().NotEmpty().WithMessage("Email Is Required")
         .EmailAddress().WithMessage("Email Is Not Valid");
                RuleFor(x => x.PasswordHash)
                   .NotEmpty().NotNull().WithMessage("Password is required")
                   .Must(ContainUpperAndLower).WithMessage("Password must contain both upper and lowercase characters")
                   .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            }
            private bool ContainUpperAndLower(string password)
            {
                return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper) && password.Any(char.IsLower);
            }
        }

    }

}
