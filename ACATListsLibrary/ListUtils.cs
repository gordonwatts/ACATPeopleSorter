using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACATListsLibrary
{
    public static class ListUtils
    {
        /// <summary>
        /// Get the list of presenters
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 1. Open up indico to the ROLES page
        /// 2. select the speaker role only
        /// 3. in chrome, cust paset the table into excel and save as a comma delimited csv file
        /// 4. Do not put in headers
        /// 
        /// Store the cvs file a directory where you run this code from.
        /// Add a header row: Name	email	association	Chairperson	Speaker	sub	convener	abstract	indico ccount
        /// </remarks>
        public static Presenters[] LoadPresenters()
        {
            using (var reader = File.OpenText("Speakers.csv"))
            {
                var p = new CsvParser(reader);
                return Enumerable.Range(0, 10000)
                    .Select(i => p.Read())
                    .Where(i => i != null)
                    .Select(i => new Presenters() { Name = i[0], Email = i[1], SpeakerInfo = i[4] })
                    .ToArray();
            }

        }

        /// <summary>
        /// Info about presenters loaded from a CSV file
        /// </summary>
        public class Presenters
        {
            public string Name;
            public string Email;
            public string SpeakerInfo;

            /// <summary>
            /// Returns the number of speakers from the speaker info field
            /// </summary>
            public int NumberPresentation
            {
                get { return SpeakerInfo.AsSpeakers(); }
            }
        }

        /// <summary>
        /// Return the number of speakers from a speaker string.
        /// </summary>
        /// <param name="indicoSpeakerString"></param>
        /// <returns></returns>
        public static int AsSpeakers(this string indicoSpeakerString)
        {
            return int.Parse(indicoSpeakerString.Substring(indicoSpeakerString.Length - 1));
        }

        /// <summary>
        /// Attempt to split properly, paying attention to quotes.
        /// </summary>
        /// <param name="cvsString"></param>
        /// <returns></returns>
        public static string[] SplitCSV(this string cvsString)
        {
            string pattern = @"""\s*,\s*""";

            // input.Substring(1, input.Length - 2) removes the first and last " from the string
            string[] tokens = System.Text.RegularExpressions.Regex.Split(cvsString, pattern);

            return tokens;
        }
    }
}
