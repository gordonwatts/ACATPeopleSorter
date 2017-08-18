using System;
using System.Linq;
using static ACATListsLibrary.CSVUtils;
using static ACATListsLibrary.ListUtils;

namespace DumpLists
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Presenters");

            var presenters = LoadPresenters();
            var plines = presenters
                .Select(p => $"{p.Email}, YES");
            WriteCSVFile("upload-presenters.csv", "Email, ACAT2017 Presenter", plines);

            Console.WriteLine("Registered");
            var registered = LoadIndicoRegistered();
            var rlines = registered
                .Select(r => $"{r.Email}, YES");
            WriteCSVFile("upload-registered.csv", "Email, ACAT2017 Indico Registered", rlines);

            Console.WriteLine("Paid");
            var paid = LoadPaid();
            var free = LoadFree();
            var paidLines = paid
                .Select(p => $"{p.Email}, YES")
                .Concat(free.Select(f => $"{f.Email}, {f.Reason}"));
            WriteCSVFile("upload-paid.csv", "Email, ACAT2017 Paid", paidLines);
        }
    }
}
