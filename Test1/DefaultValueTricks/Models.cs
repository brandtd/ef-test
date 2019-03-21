using System;
using System.Collections.Generic;
using System.Text;

namespace Test1.DefaultValueTricks
{
    public class Foo
    {
        public int FooId { get; set; }

        public ICollection<Bar> Bars { get; set; } = new List<Bar> { new Bar() };
    }
    public class Bar
    {
        public int BarId { get; set; }

        public int FooId { get; set; }
        public Foo Foo { get; set; }
    }

    public class ShoppingList
    {
        public int ShoppingListId { get; set; }
        public ICollection<Grocery> Groceries { get; set; } = new List<Grocery> { new Grocery { Name = "Milk" } };
        public string Name { get; set; }
    }

    public class Grocery
    {
        public int GroceryId { get; set; }
        public string Name { get; set; }

        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; }
    }
}
