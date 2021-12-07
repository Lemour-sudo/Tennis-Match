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
            string[] Words = line.Split(Delimiter);
            if (Words.Length != 2)
            {
                throw new InvalidDataException(
                    "Invalid CSV line entered. Line does not follow the required format: name, gender(f/m)"
                );
            }
            
            // Check name
            string Name = Words[0].Trim().ToLower();
            if (!Utility.IsAlphabetic(Name))
            {
                throw new InvalidDataException(
                    "Invalid name entered. Name must only contain alphabetic characters."
                );
            }

            // Check gender
            string Gender = Words[1].Trim().ToLower();
            if (!((Gender == "f") || (Gender == "m")))
            {
                throw new InvalidDataException(
                    "Invalid gender entered. Gender expected to be: f or m"
                );
            }

            return new CSVRecord(Name, Gender);

        }

        public List<HashSet<string>> ReadCSV(Logger LoggerObj)
        {
            HashSet<string> Females = new HashSet<string>();
            HashSet<string> Males = new HashSet<string>();
            try
            {
                int LineNumber = 1;
                foreach (string line in File.ReadLines(FilePath))
                {
                    try  
                    {
                        CSVRecord Record = ParseCSVLine(line);
                        if (Record.Gender == "f")
                        {
                            Females.Add(Record.Name);
                        }
                        else
                        {
                            Males.Add(Record.Name);
                        }
                    }
                    catch (InvalidDataException e)
                    {
                        string Message = String.Format(
                            "At line {0} in csv file: {1}",
                            LineNumber, e.Message
                        );
                        LoggerObj.WriteLineToLog(Message, LogType.Warning);
                    }

                    LineNumber++;
                }

                return new List<HashSet<string>> {Females, Males};
            }
            catch (IOException e)
            {
                string Message = "Reading input file failed:" + e.Message;
                LoggerObj.WriteLineToLog(
                    Message, LogType.Fatal
                );
                Console.WriteLine(Message);
                Console.WriteLine("\nProgram terminating.");
                Environment.Exit(0);
            }

            return new List<HashSet<string>>();
        }
    }
}
