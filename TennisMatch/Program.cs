using System;
using System.Collections.Generic;
using HelperLibrary;

namespace TennisMatch
{
    class Test
    {
        public void RunLocalTests()
        {
            const string MidStr = "matches";

            Utility Ut = new Utility();

            string Name1 = "mona";
            string Name2 = "lisa";
            Match MatchObj = new Match(Name1, Name2);

            // Test CreateMatchString
            Console.WriteLine("Test CreateMatchString:");
            Console.WriteLine(MatchObj.GetMatchString());
            Console.WriteLine();

            // Test CountMatchChars
            Console.WriteLine("Test CountMatchChars:");
            List<int> CountsList = Utility.CountChars(MatchObj.GetMatchString());
            Console.WriteLine("CountsList: " + String.Join(", ", CountsList));
            Console.WriteLine();

            // Test CountDigits
            Console.WriteLine("Test CountDigits:");
            Console.WriteLine(Utility.CountDigits(-100000000)); // expect 9
            Console.WriteLine();

            // Test ReduceDigits
            Console.WriteLine("Test ReduceDigits:");
            List<int> Counts = new List<int> {2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2};
            Console.WriteLine(Utility.ReduceDigits(Counts)); // expect: 60
            Console.WriteLine();

            // Test IsAlphabetic
            Console.WriteLine("Test IsAlphabetic:");
            string Str = "@abdc";
            Console.WriteLine(Utility.IsAlphabetic(Str));
            Console.WriteLine();

            // Test ReadCSV
            Console.WriteLine("Test ReadCSV:");
            const string IOFolder = "io_folder/";
            CSVReader CReader = new CSVReader(IOFolder + "names.csv");
            List<HashSet<string>> AllNames = CReader.ReadCSV();
            Console.WriteLine("Females: " + String.Join(", ", AllNames[0]));
            Console.WriteLine("Males: " + String.Join(", ", AllNames[1]));
            Console.WriteLine();

            // Test MatchUp
            Console.WriteLine("Test MatchUp:");
            List<string[]> Products = 
                Utility.CrossStringLists(AllNames[0], AllNames[1]);
            Console.WriteLine("Matches: " + String.Join(", ", Products));
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string MidStr = "matches";
            const int MinGoodScore = 80;     // Minimum good match score

            // // Run local tests
            // Test TestC = new Test();
            // TestC.RunLocalTests();

            const string IOFolder = "IOFolder/";
            const string InpFileName = IOFolder + "names_1.csv";
            const string OutFileName = IOFolder + "output_1.txt";

            CSVReader CReader = new CSVReader(InpFileName);

            // Read Females and Males sets from csv file
            List<HashSet<string>> AllNames = CReader.ReadCSV();

            // Cross-product the Females and Males sets
            List<string[]> Products = 
                Utility.CrossStringLists(AllNames[0], AllNames[1]);

            // Create and store matches along with their scores
            List<Match> Matches = new List<Match>();
            foreach (string[] Product in Products)
            {
                Match MatchObj = new Match(Product[0], Product[1]);

                List<int> Counts = Utility.CountChars(
                    MatchObj.GetMatchString()
                );

                MatchObj.Score = Utility.ReduceDigits(Counts);

                Matches.Add(MatchObj);
            }
            
            // Sort Matches
            Matches.Sort();

            // Save Match Results to text file
            List<string> MatchResults = new List<string>();
            foreach (Match MatchObj in Matches)
            {
                MatchResults.Add(MatchObj.GetFinalMatchString());
            }
            Utility.SaveListToFile(OutFileName, MatchResults);
        }
    }
}
