using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using Test1.DefaultValueTricks;
using Test1.ManyToMany;
using Test1.TransactionTest;

namespace Test1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ContextCachingTest.Run();
            //DefaultValueTricksTest.Run();
            TransactionsPerSecondTest.Run();
        }
    }
}