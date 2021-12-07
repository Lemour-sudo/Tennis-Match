using System;
using System.Collections.Generic;
using System.IO;
using HelperLibrary;

namespace TennisMatch
{
    class GoodMatchCSV
    {
        private static string IOFolder = "IOFolder/";
        private static string InpFileName;
        private static string OutFileName = "output.txt";
        private static string LogsPath = "Logs/";
        private static Logger LoggerObj;

        public static void Run()
        {
            FetchUserInput();

            // Start logger
            StartLogger();

            // Fetch Females and Males sets from csv file
            CSVReader CReader = new CSVReader(IOFolder + InpFileName);
            List<HashSet<string>> AllNames = CReader.ReadCSV(LoggerObj);

            // Get matches data from females and males sets
            List<Match> Matches = GetMatches(AllNames);
            
            // Count and present stats: number of females and males and matches
            ReportStats(AllNames, Matches);

            // Sort Matches
            Matches.Sort();

            // Save Match Results to text file
            SaveMatches(Matches);

            // Conclude Program
            LoggerObj.WriteLineToLog(
                "Program finsished successfully.", LogType.Info
            );
            Console.WriteLine("\nPlease checkout the 'Logs/' folder for any warnings and info.");
        }

        private static string FetchUserInput()
        {
            string UserFileName = "";

            Console.WriteLine("\nPlease input a valid csv input file-name.");
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
                    Console.WriteLine(String.Format(
                        "\nYou entered: '{0}', but a valid file-name should be at least 5 characters long, ending with: .csv",
                        UserFileName
                    ));
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine("\nPlease enter a valid file-name and press Enter to continue (Or press Ctr-C to quit):");
                    continue;
                }

                // Check if input ends with .csv
                if (UserFileName.Substring(UserFileName.Length - 4) != ".csv")
                {
                    Console.WriteLine(
                        "\nYou entered: '{0}', but a file-name ending with '.csv' is expected.",
                        UserFileName
                    );
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
                    Console.WriteLine(String.Format(
                        "\n'{0}' does not exist in '{1}'",
                        UserFileName, IOFolder
                    ));
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine(String.Format(
                        "\nPlease enter an existing csv file in '{0}' and press Enter to continue (Or press Ctr-C to quit):",
                        IOFolder
                    ));
                }

            }

            Console.WriteLine(String.Format(
                "\n'{0}' found in '{1}'!",
                UserFileName, IOFolder
            ));
            Console.WriteLine("\nProceeding to the next step ...");
            Console.WriteLine("\n-------------------------------------------------------------");

            InpFileName = UserFileName;

            return UserFileName;
        }

        private static void StartLogger()
        {
            LoggerObj = new Logger(LogsPath);
        }

        private static List<Match> GetMatches(List<HashSet<string>> AllNames)
        {
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

            return Matches;
        }

        private static void ReportStats(List<HashSet<string>> allNames, List<Match> matches)
        {
            Console.WriteLine();
            Console.WriteLine("Number of females:      " + allNames[0].Count);
            Console.WriteLine("Number of males:        " + allNames[1].Count);
            Console.WriteLine("Number of matches made: " + matches.Count);
        }

        private static void SaveMatches(List<Match> matches)
        {
            List<string> MatchResults = new List<string>();

            foreach (Match MatchObj in matches)
            {
                MatchResults.Add(MatchObj.GetFinalMatchString());
            }

            Utility.SaveListToFile(
                IOFolder+OutFileName, MatchResults, LoggerObj
            );

            LoggerObj.WriteLineToLog(
                String.Format("Output written successfully: {0}", IOFolder+OutFileName),
                LogType.Info
            );

            Console.WriteLine(
                String.Format("\nOutput written successfully to file: {0}", IOFolder+OutFileName)
            );
        }
    }
}
