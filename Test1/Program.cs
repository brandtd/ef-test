using Microsoft.EntityFrameworkCore;
using System;
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

                science.Roll = new RollList { Lesson = science };
                math.Roll = new RollList { Lesson = math };
                boring.Roll = new RollList { Lesson = boring };

                jamie.LessonPlan = new LessonPlan { Student = jamie };
                chris.LessonPlan = new LessonPlan { Student = chris };
                danny.LessonPlan = new LessonPlan { Student = danny };

                jamie.LessonPlan.Lessons.Add(science);
                jamie.LessonPlan.Lessons.Add(math);
                science.Roll.Students.Add(jamie);
                math.Roll.Students.Add(jamie);

                chris.LessonPlan.Lessons.Add(math);
                chris.LessonPlan.Lessons.Add(boring);
                math.Roll.Students.Add(chris);
                boring.Roll.Students.Add(chris);

                danny.LessonPlan.Lessons.Add(boring);
                danny.LessonPlan.Lessons.Add(science);
                boring.Roll.Students.Add(danny);
                science.Roll.Students.Add(danny);

                db.Lessons.Add(science);
                db.Lessons.Add(math);
                db.Lessons.Add(boring);
                db.SaveChanges();
            }

            DoAThing t1 = new DoAThing(new MyContext());
            DoAThing t2 = new DoAThing(new MyContext());
            Console.WriteLine("Before any shenanigans");
            t1.PrintIt();
            t2.PrintIt();

            Console.WriteLine();
            Console.WriteLine("Changing name under one context");
            t1.SetIt("new name");

            Console.WriteLine();
            Console.WriteLine("After shenanigans");
            t1.PrintIt();
            t2.PrintIt();

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
                Console.WriteLine("Lesson Plans:");
                foreach (var plan in db.LessonPlans)
                {
                    Console.WriteLine($"Lesson Plan: id: {plan.Id}");
                }

                Console.WriteLine();
                Console.WriteLine("Roll Lists:");
                foreach (var roll in db.RollLists)
                {
                    Console.WriteLine($"Roll List: id: {roll.Id}");
                }
            }
        }

        public class DoAThing
        {
            public DoAThing(MyContext db)
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