using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.CSVUtils;
using static ACATListsLibrary.ListUtils;

namespace DumpMMReceipts
{
    class Program
    {
        static void Main(string[] args)
        {
            var paid = LoadPaid();
            var banquets = LoadBanquets();

            var allPaidInfo = from p in paid
                              let b = banquets.Where(bl => bl.Email == p.Email).FirstOrDefault()
                              select new
                              {
                                  Name = p.Name,
                                  NBanquets = b == null ? 0 : b.NumberOrdered,
                                  NBCost = b == null ? "$0.00" : $"{b.NumberOrdered * 75.00:C2}",
                                  Total = b == null ? "$300.00" : $"{300.0 + b.NumberOrdered * 75.00:C2}"
                              };

            var paidLines = allPaidInfo
                .Select(p => $"{p.Name}, 1, $300.00, {p.NBanquets}, {p.NBCost}, {p.Total}");

            WriteCSVFile("mm_receipts.csv", "Name, RegistrationQ, RegistrationC, BanquetN, BanquetC, Total", paidLines);
        }
    }
}
