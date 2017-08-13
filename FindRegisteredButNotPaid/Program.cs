using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACATListsLibrary.ListUtils;
using static System.Console;
using ACATListsLibrary;

namespace FindRegisteredButNotPaid
{
    class Program
    {
        static void Main(string[] args)
        {
            const bool ask_to_update_with_associations = true;

            // Find who is missing first.
            var registeredAll = LoadIndicoRegistered();
            var paid = LoadPaid();
            var free = LoadFree();

            var registered = from r in registeredAll
                             where !free.Any(f => f.Email == r.Email)
                             select r;

            var missing = from r in registered
                          where !paid.Any(p => r.Email == p.Email)
                          select r;

            Console.WriteLine($"There are {missing.Count()} foalks that have registered but not yet paid");
            foreach (var m in missing)
            {
                WriteLine($"{m.Name}, {m.Email}");
            }

            // Next, lets see if we can match them by matching first and last names.
            WriteLine();
            WriteLine("Guesses as to who each of the missing registered folks might be by matching full name");
            var possibleMatches = from m in missing
                                  from p in paid
                                  where m.Name.ToLower() == p.Name.ToLower()
                                  group p by m;

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
            if (updated)
                return;

            // Next, lets see if we can find some more matches by looking only at last names.
            WriteLine();
            WriteLine("Guesses as to who each of the missing registered folks might be by matching last name");
            var lastnameMatches = from m in missing
                                  from p in paid
                                  where m.Name.LastName().ToLower() == p.Name.LastName().ToLower()
                                  group p by m;
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
