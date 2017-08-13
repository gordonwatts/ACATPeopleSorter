using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    .Select(i => new Presenters() { Name = i[0], Email = i[1].AsUnifiedEmail(), SpeakerInfo = i[4] })
                    .ToArray();
            }
        }

        /// <summary>
        /// Load indico registrations from registered.csv
        /// </summary>
        /// <remarks>
        /// 1. Go to the registrations listing in indico
        /// 2. Select "all"
        /// 3. Click on expert to csv
        /// 4. Save it to a file called registrations.csv
        /// </remarks>
        /// <returns></returns>
        public static IndicoRegistration[] LoadIndicoRegistered()
        {
            using (var reader = File.OpenText("registrations.csv"))
            {
                var p = new CsvParser(reader);
                return Enumerable.Range(0, 10000)
                    .Select(i => p.Read())
                    .Where(i => i != null)
                    .Select(i => new IndicoRegistration() { Name = i[1], Email = i[3].AsUnifiedEmail()})
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

        public class IndicoRegistration
        {
            public string Name;
            public string Email;
        }

        /// <summary>
        /// Normalize the email address, and if there is an alias list, use it.
        /// </summary>
        /// <param name="origEmail"></param>
        /// <returns></returns>
        public static string AsUnifiedEmail(this string origEmail)
        {
            LoadEmailAliasList();

            var t = origEmail.Trim();
            return _email_aliases.ContainsKey(t)
                ? _email_aliases[t]
                : t;
        }

        /// <summary>
        /// Remember a registered email so that we can make a mapping later on.
        /// </summary>
        /// <param name="aliasEmail"></param>
        /// <param name="registeredEmail"></param>
        public static void AddEmailAssociation(string aliasEmail, string registeredEmail)
        {
            LoadEmailAliasList();
            _email_aliases[aliasEmail] = registeredEmail;
            SaveEmailAliasList();
        }

        /// <summary>
        /// Save the list of email aliases
        /// </summary>
        private static void SaveEmailAliasList()
        {
            var lines = _email_aliases
                .Select(a => $"{a.Key} -> {a.Value}")
                .ToArray();
            File.WriteAllLines(_email_alias_filename, lines);
        }

        const string _email_alias_filename = "email_aliases.txt";

        static Dictionary<string, string> _email_aliases = null;

        /// <summary>
        /// Load in the list of registered emails
        /// </summary>
        private static void LoadEmailAliasList()
        {
            if (_email_aliases == null)
            {
                _email_aliases = new Dictionary<string, string>();
                if (File.Exists(_email_alias_filename))
                {
                    var associations = File.ReadAllLines(_email_alias_filename);
                    var splitter = new Regex(@"^([^\s]+)\s*->\s*([^\s]+)$");
                    foreach (var a in associations)
                    {
                        var m = splitter.Match(a);
                        if (!m.Success)
                        {
                            throw new ArgumentException($"Unable to parse alias line {a}.");
                        }
                        _email_aliases[m.Groups[1].Value] = m.Groups[2].Value;
                    }
                }
            }
        }


        /// <summary>
        /// Paid folks Store a list in scv called "paid.csv"
        /// </summary>
        /// <returns></returns>
        public static PaidPeople[] LoadPaid()
        {
            using (var reader = File.OpenText("paid.csv"))
            {
                var p = new CsvParser(reader);
                return Enumerable.Range(0, 10000)
                    .Select(i => p.Read())
                    .Where(i => i != null)
                    .Select(i => new PaidPeople() { FirstName = i[0], LastName = i[1], Email = i[2].AsUnifiedEmail() })
                    .ToArray();
            }
        }

        /// <summary>
        /// Someone who has paid!
        /// </summary>
        public class PaidPeople
        {
            public string FirstName;
            public string LastName;
            public string Email;

            public string Name
            {
                get { return $"{FirstName} {LastName}"; }
            }
        }
    }
}
