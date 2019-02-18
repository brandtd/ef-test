using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test1
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
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MyContext : DbContext
    {
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("inmemory");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>().HasKey(t => new { t.LessonId, t.StudentId }); // Instead of "id" property in table
        }
    }

    public class Student
    {
        public ICollection<Enrollment> Enrollments { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}