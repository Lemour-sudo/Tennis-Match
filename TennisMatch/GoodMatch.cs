using System;
using HelperLibrary;

namespace TennisMatch
{
    public class GoodMatch
    {
        private static string Name1;
        private static string Name2;

        public static void Run()
        {
            FetchUserInput();

            Match MatchObj = new Match(Name1, Name2);

            MatchObj.Score = ScoreMatch(MatchObj);

            // Report Match score
            Console.WriteLine("\nHere's the match result:");
            Console.WriteLine("\t" + MatchObj.GetFinalMatchString());
        }

        private static void FetchUserInput()
        {
            const string WarningMessage = 
                "\nInvalid name entered.\nPlease enter a name that is strictly alphabetic. (Or press Ctr-C to quit):";

            Console.WriteLine("\nYou are prompted to enter two names to match.");
            Console.WriteLine("Each name must strictly be alphabetic.");
            Console.WriteLine("\n-------------------------------------------------------------");

            Console.WriteLine("\nPlease enter the first name below:");
            while (true)
            {
                string InputName = Console.ReadLine();

                if (Utility.IsAlphabetic(InputName))
                {
                    Name1 = InputName.ToLower();
                    break;
                }

                Console.WriteLine(WarningMessage);
            }

            Console.WriteLine("\nPlease enter the second name below:");
            while (true)
            {
                string InputName = Console.ReadLine();

                if (Utility.IsAlphabetic(InputName))
                {
                    Name2 = InputName.ToLower();
                    break;
                }

                Console.WriteLine(WarningMessage);
            }
        }
    
        private static int ScoreMatch(Match MatchObj)
        {
            return Utility.ReduceDigits(
                Utility.CountChars(
                    MatchObj.GetMatchString()
                )
            );
        }
    }
}
