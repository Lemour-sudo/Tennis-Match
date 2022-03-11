using System;
using System.Collections.Generic;
using System.IO;
using HelperLibrary;

namespace TennisMatch
{
    class GoodMatchCSV
    {
        private static string ioFolder = "IOFolder/";
        private static string inpFileName;
        private static string outFileName = "output.txt";
        private static string logsPath = "Logs/";
        private static Logger loggerObj;

        public static void Run()
        {
            FetchUserInput();

            // Start logger
            StartLogger();

            // Fetch Females and Males sets from csv file
            CSVReader cReader = new CSVReader(ioFolder + inpFileName);
            List<HashSet<string>> allNames = cReader.ReadCSV(loggerObj);

            // Get matches data from females and males sets
            List<Match> matches = GetMatches(allNames);
            
            // Count and present stats: number of females and males and matches
            ReportStats(allNames, matches);

            // Sort matches
            matches.Sort();

            // Save match Results to text file
            SaveMatches(matches);

            // Conclude Program
            loggerObj.WriteLineToLog(
                "Program finsished successfully.", LogType.Info
            );
            Console.WriteLine("\nPlease checkout the 'Logs/' folder for any warnings and info.");
        }

        private static string FetchUserInput()
        {
            string userFileName = "";

            Console.WriteLine("\nPlease input a valid csv input file-name.");
            Console.WriteLine("Make sure the csv file is available in the 'IOFolder' folder.");
            Console.WriteLine("The file-name should end with: .csv");
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("\nEnter file name below and hit Enter to continue:");

            bool validInput = false;
            while (!validInput)
            {
                userFileName = Console.ReadLine();

                // Check if input is at least 5 characters long
                if (userFileName.Length < 5)
                {
                    Console.WriteLine(String.Format(
                        "\nYou entered: '{0}', but a valid file-name should be at least 5 characters long, ending with: .csv",
                        userFileName
                    ));
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine("\nPlease enter a valid file-name and press Enter to continue (Or press Ctr-C to quit):");
                    continue;
                }

                // Check if input ends with .csv
                if (userFileName.Substring(userFileName.Length - 4) != ".csv")
                {
                    Console.WriteLine(
                        "\nYou entered: '{0}', but a file-name ending with '.csv' is expected.",
                        userFileName
                    );
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine("\nPlease enter a valid file-name and press Enter to continue (Or press Ctr-C to quit):");
                    continue;
                }

                // Check if file exists
                if (File.Exists(ioFolder + userFileName))
                {
                    validInput = true;
                }
                else if (File.Exists("../" + ioFolder + userFileName))
                {
                    ioFolder = "../" + ioFolder;
                    validInput = true;
                }
                else
                {
                    Console.WriteLine(String.Format(
                        "\n'{0}' does not exist in '{1}'",
                        userFileName, ioFolder
                    ));
                    Console.WriteLine("\n-------------------------------------------------------------");
                    Console.WriteLine(String.Format(
                        "\nPlease enter an existing csv file in '{0}' and press Enter to continue (Or press Ctr-C to quit):",
                        ioFolder
                    ));
                }

            }

            Console.WriteLine(String.Format(
                "\n'{0}' found in '{1}'!",
                userFileName, ioFolder
            ));
            Console.WriteLine("\nProceeding to the next step ...");
            Console.WriteLine("\n-------------------------------------------------------------");

            inpFileName = userFileName;

            return userFileName;
        }

        private static void StartLogger()
        {
            loggerObj = new Logger(logsPath);
        }

        private static List<Match> GetMatches(List<HashSet<string>> allNames)
        {
            // Cross-product the females and males sets
            List<string[]> products = 
                Utility.CrossStringLists(allNames[0], allNames[1]);

            // Create and store matches along with their scores
            List<Match> matches = new List<Match>();
            foreach (string[] product in products)
            {
                Match matchObj = new Match(product[0], product[1]);

                List<int> counts = Utility.CountChars(
                    matchObj.GetMatchString()
                );

                matchObj.Score = Utility.ReduceDigits(counts);

                matches.Add(matchObj);
            }

            return matches;
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
            List<string> matchResults = new List<string>();

            foreach (Match matchObj in matches)
            {
                matchResults.Add(matchObj.GetFinalMatchString());
            }

            Utility.SaveListToFile(
                ioFolder+outFileName, matchResults, loggerObj
            );

            loggerObj.WriteLineToLog(
                String.Format("Output written successfully: {0}", ioFolder+outFileName),
                LogType.Info
            );

            Console.WriteLine(
                String.Format("\nOutput written successfully to file: {0}", ioFolder+outFileName)
            );
        }
    }
}
