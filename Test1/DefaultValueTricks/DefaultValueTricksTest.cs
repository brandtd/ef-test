using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test1.DefaultValueTricks
{
    public static class DefaultValueTricksTest
    {
        public static void Run()
        {
            File.Delete("test.db");
            using (var db = new DefaultValueTricksDatabase())
            {
                db.Database.EnsureCreated();
                Foo foo = new Foo();
                db.Foos.Add(foo);

                foo.Bars = new List<Bar>
                {
                    new Bar()
                };
                db.SaveChanges();
            }

            using (var db = new DefaultValueTricksDatabase())
            {
                Console.WriteLine($"Num Foo: {db.Foos.Count()}");
                Console.WriteLine($"Num Bar: {db.Bars.Count()}");
            }

            using (var db = new DefaultValueTricksDatabase())
            {
                Foo foo = db.Foos.First();

                Console.WriteLine($"Num Foo: {db.Foos.Count()}");
                Console.WriteLine($"Num Bar: {db.Bars.Count()}");
            }

            Console.ReadLine();
        }
    }
}
