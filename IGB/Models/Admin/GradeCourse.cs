using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Admin
{
    public class GradeCourse
    {
        [Key]
        public long Id { get; set; }
        public long? GradeId { get; set; }
        public long? CourseId { get; set; }


        [ForeignKey("GradeId")]
        public virtual Grade? Grades { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Courses { get; set; }
    }
}
