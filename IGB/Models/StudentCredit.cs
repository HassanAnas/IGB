using IGB.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models
{
    public class StudentCredit
    {
        [Key]
        public long Id { get; set; }
        public double? Credit { get; set; }
        public string? StudentId { get; set; }
        public long? CourseBookingId { get; set; }

        [ForeignKey("StudentId")]
        public virtual ApplicationUser? ApplicationUsers { get; set; }
        [ForeignKey("CourseBookingId")]
        public virtual CourseBooking? CourseBooking { get; set; }
    }
}
