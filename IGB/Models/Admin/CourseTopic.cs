using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGB.Models.Admin
{
    public class CourseTopic
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Courses { get; set; }
    }
}
