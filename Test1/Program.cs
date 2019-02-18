using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new MyContext())
            {
                Lesson science = new Lesson { Name = "Science" };
                Lesson math = new Lesson { Name = "Math" };
                Lesson boring = new Lesson { Name = "Other Boring Stuff" };

                Student jamie = new Student { Name = "Jamie" };
                Student chris = new Student { Name = "Chris" };
                Student danny = new Student { Name = "Danny" };

                List<Enrollment> enrollments = new List<Enrollment>();

                enrollments.Add(new Enrollment { Student = jamie, Lesson = science });
                enrollments.Add(new Enrollment { Student = jamie, Lesson = math });
                enrollments.Add(new Enrollment { Student = chris, Lesson = math });
                enrollments.Add(new Enrollment { Student = chris, Lesson = boring });
                enrollments.Add(new Enrollment { Student = danny, Lesson = boring });
                enrollments.Add(new Enrollment { Student = danny, Lesson = science });

                db.Lessons.Add(science);
                db.Lessons.Add(math);
                db.Lessons.Add(boring);
                db.Students.Add(jamie);
                db.Students.Add(chris);
                db.Students.Add(danny);
                db.Enrollments.AddRange(enrollments);
                db.SaveChanges();
            }

            RenameWithoutChangingContext t1 = new RenameWithoutChangingContext(new MyContext());
            RenameWithoutChangingContext t2 = new RenameWithoutChangingContext(new MyContext());
            Console.WriteLine("Before any shenanigans");
            t1.PrintIt();
            t2.PrintIt();

            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine("Changing name under one context");
            t1.SetIt("new name");

            Console.WriteLine();
            Console.WriteLine("After shenanigans");
            t1.PrintIt();
            t2.PrintIt();

            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine();
            Console.WriteLine("Before removing student (without loading enrollments):");
            PrintDatabase();

            using (var db = new MyContext())
            {
                db.Remove(db.Students.Where(s => s.Id == 2).First());
                db.SaveChanges();
            }

            Console.WriteLine();
            Console.WriteLine("After removing student (without loading enrollments):");
            PrintDatabase();

            Console.WriteLine();
            Console.WriteLine("================================");
            Console.WriteLine();
            Console.WriteLine("Before removing student (WITH loading enrollments):");
            PrintDatabase();

            using (var db = new MyContext())
            {
                db.Remove(db.Students.Where(s => s.Id == 1).Include(s => s.Enrollments).First());
                db.SaveChanges();
            }

            Console.WriteLine();
            Console.WriteLine("After removing student (WITH loading enrollments):");
            PrintDatabase();

            Console.WriteLine();
            using (var db = new MyContext())
            {
                Console.WriteLine("Remaining student:");
                foreach (var student in db.Students.Include(s => s.Enrollments).ThenInclude(e => e.Lesson).ToList())
                {
                    Console.WriteLine($"  Name: {student.Name}");
                    foreach (var lesson in student.Enrollments.Select(e => e.Lesson))
                    {
                        Console.WriteLine($"  Lesson: {lesson.Name}");
                    }
                }
            }

            Console.ReadLine();
        }

        public static void PrintDatabase()
        {
            using (var db = new MyContext())
            {
                Console.WriteLine("Lessons:");
                foreach (var lesson in db.Lessons)
                {
                    Console.WriteLine($"Lesson: id: {lesson.Id}, name: {lesson.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("Students:");
                foreach (var student in db.Students)
                {
                    Console.WriteLine($"Student: id: {student.Id}, name: {student.Name}");
                }

                Console.WriteLine();
                Console.WriteLine("Enrollments:");
                foreach (var enrollment in db.Enrollments)
                {
                    Console.WriteLine($"Enrollment: id: {db.Entry(enrollment).Metadata.FindPrimaryKey()}");
                }
            }
        }

        public class RenameWithoutChangingContext
        {
            public RenameWithoutChangingContext(MyContext db)
            {
                _db = db;
            }

            public void PrintIt()
            {
                Console.WriteLine($"Name under context {_db.GetHashCode()}: {_db.Students.Where(s => s.Id == 2).First().Name}");
            }

            public void SetIt(string name)
            {
                Console.WriteLine($"Changing name under context {_db.GetHashCode()}");
                _db.Students.Where(s => s.Id == 2).First().Name = name;
                _db.SaveChanges();
            }

            private MyContext _db;
        }
    }
}