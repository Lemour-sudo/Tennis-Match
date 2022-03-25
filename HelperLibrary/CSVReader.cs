using System;
using System.Collections.Generic;
using System.IO;

namespace HelperLibrary
{
    public class CSVRecord
    {
        public string Name { get; set; }
        public string Gender { get; set; }

        public CSVRecord(string name, string gender)
        {
            Name = name;
            Gender = gender;
        }
    }

    public class CSVReader
    {
        public string FilePath { get; }
        public char Delimiter { get; }

        public CSVReader(string filePath, char delimiter=',')
        {
            FilePath = filePath;
            Delimiter = delimiter;
        }

        public CSVRecord ParseCSVLine(string line)
        {
            string[] words = line.Split(Delimiter);
            if (words.Length != 2)
            {
                throw new InvalidDataException(
                    "Invalid CSV line entered. Line does not follow the required format: name, gender(f/m)"
                );
            }
            
            // Check name
            string name = words[0].Trim().ToLower();
            if (!Utility.IsAlphabetic(name))
            {
                throw new InvalidDataException(
                    "Invalid name entered. Name must only contain alphabetic characters."
                );
            }

            // Check gender
            string gender = words[1].Trim().ToLower();
            if (!((gender == "f") || (gender == "m")))
            {
                throw new InvalidDataException(
                    "Invalid gender entered. Gender expected to be: f or m"
                );
            }

            return new CSVRecord(name, gender);

        }

        public List<HashSet<string>> ReadCSV(Logger loggerObj)
        {
            HashSet<string> females = new HashSet<string>();
            HashSet<string> males = new HashSet<string>();
            try
            {
                int lineNumber = 1;
                foreach (string line in File.ReadLines(FilePath))
                {
                    try  
                    {
                        CSVRecord record = ParseCSVLine(line);
                        if (record.Gender == "f")
                        {
                            females.Add(record.Name);
                        }
                        else
                        {
                            males.Add(record.Name);
                        }
                    }
                    catch (InvalidDataException e)
                    {
                        string message = String.Format(
                            "At line {0} in csv file: {1}",
                            lineNumber, e.Message
                        );
                        loggerObj.WriteLineToLog(message, LogType.Warning);
                    }

                    lineNumber++;
                }

                return new List<HashSet<string>> {females, males};
            }
            catch (IOException e)
            {
                string message = "Reading input file failed:" + e.Message;
                loggerObj.WriteLineToLog(
                    message, LogType.Fatal
                );
                Console.WriteLine(message);
                Console.WriteLine("\nProgram terminating.");
                Environment.Exit(0);
            }

            return new List<HashSet<string>>();
        }
    }
}
