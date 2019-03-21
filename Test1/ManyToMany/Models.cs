using System.Collections.Generic;

namespace Test1.ManyToMany
{
    /// <summary>Join table to allow many-to-many between Lesson and Student</summary>
    public class Enrollment
    {
        public Lesson Lesson { get; set; }
        public int LessonId { get; set; }
        
        public Student Student { get; set; }
        public int StudentId { get; set; }
    }

    public class Lesson
    {
        public ICollection<Enrollment> Enrollments { get; set; }
        public int LessonId { get; set; }
        public string Name { get; set; }
    }

    public class Student
    {
        public ICollection<Enrollment> Enrollments { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
    }
}