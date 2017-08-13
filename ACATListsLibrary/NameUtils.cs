using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACATListsLibrary
{
    public static class NameUtils
    {
        /// <summary>
        /// Return the last name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string LastName(this string name)
        {
            var sp = name.LastIndexOf(' ');
            if (sp >= 0)
            {
                return name.Substring(sp + 1);
            } else
            {
                return name;
            }
        }
    }
}
