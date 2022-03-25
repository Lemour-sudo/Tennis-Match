using System;
using HelperLibrary;

namespace TennisMatch
{
    class Program
    {   
        static void Main(string[] args)
        {
            Console.WriteLine("\nWelcome to Players-MatchUp!");
            Console.WriteLine("===========================\n");

            SetUpMainVars();

            if (ChooseProgram() == "good-match")
            {
                GoodMatch.Run();
            }
            else
            {
                GoodMatchCSV.Run();
            }

            Console.WriteLine("\n\nProgram complete! May the Game be with You ;)\n");
        }

        private static void SetUpMainVars()
        {
            // Setup Match static variables
            Match.MidStr = "matches";
            Match.MinGoodScore = 80;
        }

        private static string ChooseProgram()
        {
            Console.WriteLine("\nFor a simple Good-Match between two stings, type 1 and hit Enter.");
            Console.WriteLine("For Good-Match on a CSV file, type 2 and hit Enter.");
            Console.WriteLine("\n-------------------------------------------------------------");

            Console.WriteLine("\nEnter 1 or 2 to select a program:");
            while (true)
            {
                string inputChoice = Console.ReadLine();

                if (inputChoice == "1")
                {
                    Console.WriteLine("\nGood! You chose a simple Good-Match on two strings.");
                    Console.WriteLine("\n-------------------------------------------------------------");
                    return "good-match";
                }
                else if (inputChoice == "2")
                {
                    Console.WriteLine("\nGreat! You chose Good-Match on CSV.");
                    Console.WriteLine("\n-------------------------------------------------------------");
                    return "good-match-csv";
                }

                Console.WriteLine("\nInvalid input entered. Please enter either 1 or 2. (or hit Ctr-C to quit):");
            }

        }
    }
}
