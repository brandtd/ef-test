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
        public RollList Roll { get; set; }
    }

    /// <summary>Join table to allow many-to-many between Lesson and Student</summary>
    public class LessonPlan
    {
        public int Id { get; set; }
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public Student Student { get; set; }
        public int StudentId { get; set; } // Necessary to identify LessonPlan as child of Student?
    }

    public class MyContext : DbContext
    {
        public DbSet<LessonPlan> LessonPlans { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<RollList> RollLists { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("inmemory");
        }
    }

    /// <summary>Join table to allow many-to-many between Lesson and Student</summary>
    public class RollList
    {
        public int Id { get; set; }
        public Lesson Lesson { get; set; }
        public int LessonId { get; set; } // Necessary to identify RollList as child of Lesson?

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }

    public class Student
    {
        public int Id { get; set; }
        public LessonPlan LessonPlan { get; set; }
        public string Name { get; set; }
    }
}