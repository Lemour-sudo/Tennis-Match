using System;
using HelperLibrary;

namespace TennisMatch
{
    public class GoodMatch
    {
        private static string name1;
        private static string name2;

        public static void Run()
        {
            FetchUserInput();

            Match matchObj = new Match(name1, name2);

            matchObj.Score = ScoreMatch(matchObj);

            // Report match score
            Console.WriteLine("\nHere's the match result:");
            Console.WriteLine("\t" + matchObj.GetFinalMatchString());
        }

        private static void FetchUserInput()
        {
            const string warningMessage = 
                "\nInvalid name entered.\nPlease enter a name that is strictly alphabetic. (Or press Ctr-C to quit):";

            Console.WriteLine("\nYou are prompted to enter two names to match.");
            Console.WriteLine("Each name must strictly be alphabetic.");
            Console.WriteLine("\n-------------------------------------------------------------");

            Console.WriteLine("\nPlease enter the first name below:");
            while (true)
            {
                string inputName = Console.ReadLine();

                if (Utility.IsAlphabetic(inputName))
                {
                    name1 = inputName.ToLower();
                    break;
                }

                Console.WriteLine(warningMessage);
            }

            Console.WriteLine("\nPlease enter the second name below:");
            while (true)
            {
                string inputName = Console.ReadLine();

                if (Utility.IsAlphabetic(inputName))
                {
                    name2 = inputName.ToLower();
                    break;
                }

                Console.WriteLine(warningMessage);
            }
        }
    
        private static int ScoreMatch(Match matchObj)
        {
            return Utility.ReduceDigits(
                Utility.CountChars(
                    matchObj.GetMatchString()
                )
            );
        }
    }
}
