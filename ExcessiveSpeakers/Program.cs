using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.ListUtils;

namespace ExcessiveSpeakers
{
    class Program
    {
        static void Main(string[] args)
        {
            const int maxPresentations = 3;

            var presenters = LoadPresenters();
            var toomany = from p in presenters where p.NumberPresentation >= maxPresentations select p;

            foreach (var p in toomany)
            {
                Console.WriteLine($"{p.Name}, {p.Email}, {p.NumberPresentation}");
            }
        }
    }
}
