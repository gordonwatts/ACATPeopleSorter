using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACATListsLibrary;
using static ACATListsLibrary.ListUtils;
using System.IO;

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

        /// <summary>
        /// Write out a csv file with the proper info in it
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="plines"></param>
        private static void WriteCSVFile(string fname, string headerLine, IEnumerable<string> plines)
        {
            using (var wr = File.CreateText(fname))
            {
                wr.WriteLine(headerLine);
                foreach (var l in plines)
                {
                    wr.WriteLine(l);
                }
            }
        }
    }
}
