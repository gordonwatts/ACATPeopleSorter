using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.ListUtils;
using static ACATListsLibrary.CSVUtils;

namespace DumpMMRegistration
{
    class Program
    {
        static void Main(string[] args)
        {
            var registeredAll = LoadIndicoRegistered();
            var paid = LoadPaid();
            var free = LoadFree();
            var banquets = LoadBanquets();

            var allInfo = from r in registeredAll
                          let banquetID = banquets.Where(b => b.Email == r.Email).FirstOrDefault()
                          let banquetString = banquetID == null ? "" : $"{banquetID.NumberOrdered} Banquet"
                          let reciptID = paid.Where(p => p.Email == r.Email).FirstOrDefault()
                          let receptString = reciptID == null ? "" : "Receipt"
                          let freeID = free.Where(p => p.Email == r.Email).FirstOrDefault()
                          let freeComment = freeID == null ? "" : freeID.Reason
                          select new
                          {
                              Name = r.Name,
                              Banquet = $"{banquetString}",
                              Receipt = receptString,
                              Comment = freeComment
                          };
                          

            var rlines = allInfo
                .Where(a => !a.Comment.Contains("BOGUS"))
                .Select(p => $"{p.Name},{p.Banquet},{p.Receipt},{p.Comment}");

            WriteCSVFile("mm_registered.csv", "Name, Banquet, Receipt, Comment", rlines);
        }
    }
}
