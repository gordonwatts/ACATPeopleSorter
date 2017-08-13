using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.ListUtils;
using ACATListsLibrary;
using static System.Console;

namespace FindPaidButNotRegistered
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find who is missing first.
            var registered = LoadIndicoRegistered();
            var paid = LoadPaid();

            var missing = from p in paid
                          where !registered.Any(r => r.Email == p.Email)
                          select p;

            Console.WriteLine($"There are {missing.Count()} foalks that have registered but not yet paid");
            foreach (var m in missing)
            {
                WriteLine($"{m.Name}, {m.Email}");
            }
        }
    }
}
