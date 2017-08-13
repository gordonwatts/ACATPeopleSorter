using ACATListsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.ListUtils;
using static System.Console;

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
            // SHould we prompt or just dump?
            const bool ask_to_update_with_associations = true;

            // Fetch the presenters and registered lists, and go from there.
            var presenters = LoadPresenters();
            var registered = LoadIndicoRegistered();

            var missing = from p in presenters
                          where !registered.Any(r => r.Email == p.Email)
                          select p;

            Console.WriteLine($"There are {missing.Count()} presenters not yet registered");
            foreach (var m in missing)
            {
                Console.WriteLine($"{m.Name}, {m.Email}");
            }

            // Next, lets see if we can match them by matching first and last names.
            WriteLine();
            WriteLine("Guesses as to who each of the missing registered folks might be by matching full name");
            var possibleMatches = from m in missing
                                  from r in registered
                                  where m.Name == r.Name
                                  group r by m;

            bool updated = false;
            foreach (var missingGuesses in possibleMatches)
            {
                WriteLine($"{missingGuesses.Key.Name} - {missingGuesses.Key.Email} might be:");
                foreach (var g in missingGuesses)
                {
                    WriteLine($"  {g.Name} - {g.Email}");

                    if (ask_to_update_with_associations)
                    {
                        Write("  --> Update email assocation file? [y/n]: ");
                        while (true)
                        {
                            var k = ReadKey();
                            var goodK = k.KeyChar.ToString().ToLower();
                            if (goodK == "y")
                            {
                                AddEmailAssociation(missingGuesses.Key.Email, g.Email);
                                updated = true;
                                break;
                            } else if (goodK == "n")
                            {
                                break;
                            }
                        }
                        WriteLine();
                    }
                }
            }
            if (updated)
                return;

            // Next, lets see if we can find some more matches by looking only at last names.
            WriteLine();
            WriteLine("Guesses as to who each of the missing registered folks might be by matching last name");
            var lastnameMatches = from m in missing
                                  from r in registered
                                  where m.Name.LastName() == r.Name.LastName()
                                  group r by m;
            foreach (var missingGuesses in lastnameMatches)
            {
                WriteLine($"{missingGuesses.Key.Name} - {missingGuesses.Key.Email} might be:");
                foreach (var g in missingGuesses)
                {
                    WriteLine($"  {g.Name} - {g.Email}");

                    if (ask_to_update_with_associations)
                    {
                        Write("  --> Update email assocation file? [y/n]: ");
                        while (true)
                        {
                            var k = ReadKey();
                            var goodK = k.KeyChar.ToString().ToLower();
                            if (goodK == "y")
                            {
                                AddEmailAssociation(missingGuesses.Key.Email, g.Email);
                                updated = true;
                                break;
                            }
                            else if (goodK == "n")
                            {
                                break;
                            }
                        }
                        WriteLine();
                    }
                }
            }

        }
    }
}
