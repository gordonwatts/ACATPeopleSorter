using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.ListUtils;

namespace FindMissingPresenters
{
    class Program
    {
        /// <summary>
        /// Dump out a list of people that are listed as presenters but have
        /// not registered in indico yet.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var presenters = LoadPresenters();
        }
    }
}
