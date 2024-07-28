using FluentValidation;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Tutor
{
    public class TutorDocument
    {
        [Key]
        public long Id { get; set; }
        public string? TutorId { get; set; }
        public string? Information { get; set; }
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public byte[]? File { get; set; }




        [ForeignKey("TutorId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }


        public class TutorDocumentValidator : AbstractValidator<TutorDocument>
        {
            public TutorDocumentValidator()
            {
                RuleFor(x => x.Information).NotNull().NotEmpty().WithMessage("Information Is Required");
                RuleFor(x => x.FileType).NotNull().NotEmpty().WithMessage("File Type Is Required");
                RuleFor(x => x.FileName).NotNull().NotEmpty().WithMessage("File Name Is Required");
                RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("File Is Required");           

            }
        }

    }
}
