using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Test1.TransactionTest
{
    public static class TransactionsPerSecondTest
    {
        private static TimeSpan run(int numTransactions, DbContextOptions options)
        {
            using (var db = new TransactDatabase(options))
            {
                db.Database.EnsureCreated();
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            for (int i = 0; i < numTransactions; ++i)
            {
                using (var db = new TransactDatabase(options))
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

                using (var db = new TransactDatabase(options))
                {
                    db.Lessons.RemoveRange(db.Lessons.ToList());
                    db.Students.RemoveRange(db.Students.ToList());
                    db.Enrollments.RemoveRange(db.Enrollments.ToList());
                    db.SaveChanges();
                }
            }
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        public static void Run()
        {
            File.Delete("test.db");
            int numTransactions = 2000;

            InMemoryDatabaseRoot root = new InMemoryDatabaseRoot();
            var dbOptionsBuilder = new DbContextOptionsBuilder();
            dbOptionsBuilder.UseInMemoryDatabase("inmemory", root);

            Console.WriteLine("Running in memory test");
            TimeSpan memoryTimeSpan = run(numTransactions, dbOptionsBuilder.Options);

            dbOptionsBuilder = new DbContextOptionsBuilder();
            dbOptionsBuilder.UseSqlite("Data Source=test.db");

            Console.WriteLine("Running in SQL test");
            TimeSpan sqliteTimeSpan = run(numTransactions, dbOptionsBuilder.Options);

            Console.WriteLine($"Inmemory: {numTransactions / memoryTimeSpan.TotalSeconds} tx/s");
            Console.WriteLine($"sqlite:   {numTransactions / sqliteTimeSpan.TotalSeconds} tx/s");
            Console.ReadLine();
        }
    }
}
