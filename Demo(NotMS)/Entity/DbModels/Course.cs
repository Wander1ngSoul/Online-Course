using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Entity.Models
{
    public class Course
    {
        public long CourseId { get; set; }
        public string NameCourse { get; set; }
        public int Hours { get; set; }
        public int Cost { get; set; }
        public string inforamtion { get; set; }
        public string? Img { get; set; }
        //[NotMapped]
        //public bool? IsInCart { get; set; }

    }
}
