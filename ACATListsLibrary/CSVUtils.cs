using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACATListsLibrary
{
    public static class CSVUtils
    {
        /// <summary>
        /// Write out a csv file with the proper info in it
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="plines"></param>
        public static void WriteCSVFile(string fname, string headerLine, IEnumerable<string> plines)
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
