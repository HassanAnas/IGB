using FluentValidation;
using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Admin
{
    public class AdminDocument
    {
        [Key]
        public long Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? FileType { get; set; }
        public string? FileName { get; set; }
        public byte[]? File { get; set; }




        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }


        public class AdminDocumenttValidator : AbstractValidator<AdminDocument>
        {
            public AdminDocumenttValidator()
            {
                RuleFor(x => x.FileType).NotNull().NotEmpty().WithMessage("File Type Is Required");
                RuleFor(x => x.FileName).NotNull().NotEmpty().WithMessage("File Name Is Required");
                RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("File Is Required");

            }
        }
    }
}
