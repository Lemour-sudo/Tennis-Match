using System;
using System.Collections.Generic;
using System.IO;
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
        private static string IOFolder = "IOFolder/";
        private static string InpFileName;
        private static string OutFileName = "output.txt";

        private static string FetchUserInput()
        {
            string UserFileName = "";

            Console.WriteLine("Please input a valid csv input file-name.");
            Console.WriteLine("Make sure the csv file is available in the 'IOFolder' folder.");
            Console.WriteLine("The file-name should end with: .csv");
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("\nEnter file name below and hit Enter to continue:");

            bool ValidInput = false;
            while (!ValidInput)
            {
                UserFileName = Console.ReadLine();

                // Check if input is at least 5 characters long
                if (UserFileName.Length < 5)
                {
                    Console.WriteLine($"\nYou entered: '{UserFileName}', but a valid file-name should be at least 5 characters long, ending with: .csv");
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine("\nPlease enter a valid file-name and press Enter to continue (Or press Ctr-C to quit):");
                    continue;
                }

                // Check if input ends with .csv
                if (UserFileName.Substring(UserFileName.Length - 4) != ".csv")
                {
                    Console.WriteLine($"\nYou entered: '{UserFileName}', but a file-name ending with '.csv' is expected.");
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine("\nPlease enter a valid file-name and press Enter to continue (Or press Ctr-C to quit):");
                    continue;
                }

                // Check if file exists
                if (File.Exists(IOFolder + UserFileName))
                {
                    ValidInput = true;
                }
                else if (File.Exists("../" + IOFolder + UserFileName))
                {
                    IOFolder = "../" + IOFolder;
                    ValidInput = true;
                }
                else
                {
                    Console.WriteLine($"\n'{UserFileName}' does not exist in '{IOFolder}'");
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine($"\nPlease enter an existing csv file in '{IOFolder}' and press Enter to continue (Or press Ctr-C to quit):");
                }

            }

            Console.WriteLine($"\n'{UserFileName}' found in '{IOFolder}'!");
            Console.WriteLine("\nProceeding to the next step...");
            Console.WriteLine("\n-------------------------------------------------------------\n");

            InpFileName = UserFileName;

            return UserFileName;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\nWelcome to Players-MatchUp!");
            Console.WriteLine("===========================\n");

            // const string MidStr = "matches";
            // const int MinGoodScore = 80;     // Minimum good match score

            // // Run local tests
            // Test TestC = new Test();
            // TestC.RunLocalTests();

            FetchUserInput();

            CSVReader CReader = new CSVReader(IOFolder + InpFileName);

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
            Utility.SaveListToFile(IOFolder+OutFileName, MatchResults);
        }
    }
}
