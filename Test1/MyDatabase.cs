using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test1
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }

    /// <summary>Join table to allow many-to-many between Lesson and Student</summary>
    public class Enrollment
    {
        public int Id { get; set; }
        public Lesson Lesson { get; set; }
        public int LessonId { get; set; } // Necessary to identify Enrollment as child of Lesson?
        public Student Student { get; set; }
        public int StudentId { get; set; } // Necessary to identify Enrollment as child of Student?
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }

    public class MyContext : DbContext
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("inmemory");
        }
    }
}