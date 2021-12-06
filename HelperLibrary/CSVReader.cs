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
                Console.WriteLine("Invalid line");
            }
            
            // Check name
            string Name = Words[0].Trim().ToLower();
            if (!Utility.IsAlphabetic(Name))
            {
                Console.WriteLine("Name not alphabetic");
            }

            // Check gender
            string Gender = Words[1].Trim().ToLower();
            if (!((Gender == "f") || (Gender == "m")))
            {
                Console.WriteLine("Gender is invalid");
            }

            return new CSVRecord(Name, Gender);

        }

        public List<HashSet<string>> ReadCSV()
        {
            HashSet<string> Females = new HashSet<string>();
            HashSet<string> Males = new HashSet<string>();
            try
            {
                
                foreach (string line in File.ReadLines(FilePath))
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

                return new List<HashSet<string>> {Females, Males};
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return new List<HashSet<string>>();
        }
    }
}
