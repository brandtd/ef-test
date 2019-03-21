using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test1.DefaultValueTricks
{
    public class DefaultValueTricksDatabase : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Grocery> Groceries { get; set; }

        public DbSet<Foo> Foos { get; set; }
        public DbSet<Bar> Bars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=test.db"); //.UseInMemoryDatabase("inmemory");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


    }
}
