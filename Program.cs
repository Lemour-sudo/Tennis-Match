﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace TennisMatch
{
    class CSVRecord
    {
        public string Name { get; set; }
        public string Gender { get; set; }

        public CSVRecord(string name, string gender)
        {
            Name = name;
            Gender = gender;
        }
    }

    class Utility
    {
        public string CreateMatchString(string name1, string name2, string midStr)
        {
            return name1 + midStr + name2;
        }

        public List<int> CountChars(string name1, string name2, string midStr)
        {
            name1 = name1.ToLower().Trim();
            name2 = name2.ToLower().Trim();

            List<int> CountsList = new List<int>();

            Dictionary<char, int> CharsDict = 
                new Dictionary<char, int>();
            
            string Str = CreateMatchString(name1, name2, midStr);
            foreach (char c in Str)
            {
                if (CharsDict.ContainsKey(c))
                {
                    CharsDict[c] += 1;
                }
                else
                {
                    CharsDict.Add(c, 1);
                }
            }

            HashSet<char> SeenChars = new HashSet<char>();
            foreach (char c in Str)
            {
                if (!SeenChars.Contains(c))
                {
                    CountsList.Add(CharsDict[c]);
                    SeenChars.Add(c);
                }
            }

            return CountsList;
        }

        public int CountDigits(int number)
        {
            return number == 0 ? 1 : (int) Math.Floor(Math.Log10(Math.Abs(number)) + 1);
        }

        public int ReduceDigits(List<int> counts)
        {
            while (counts.Count > 2)
            {
                int i = 0;
                int j = counts.Count - 1;
                List<int> NewCounts = new List<int>();
                while (i <= j)
                {
                    int Num;
                    // Add the leftmost and righmost numbers
                    if (i == j)
                    {
                        Num = counts[i];
                    }
                    else
                    {
                        Num = counts[i] + counts[j];
                    }

                    // Check if Num is a single- or 2-digit number
                    if (CountDigits(Num) > 1)
                    {
                        NewCounts.Add((int) (Num / 10));
                        NewCounts.Add(Num % 10);
                    }
                    else
                    {
                        NewCounts.Add(Num);
                    }

                    i++;
                    j--;
                }

                counts = NewCounts;
            }

            if (counts.Count == 2)
            {
                return counts[0] * 10 + counts[1];
            }

            return counts[0];
        }

        public bool IsAlphabetic(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");

        }
        
        public CSVRecord ParseCSVLine(string line, string delimiter=",")
        {
            string[] Words = line.Split(delimiter);
            if (Words.Length != 2)
            {
                Console.WriteLine("Invalid line");
            }
            
            // Check name
            string Name = Words[0].Trim().ToLower();
            if (!IsAlphabetic(Name))
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

        public List<HashSet<string>> ReadCSV(string filePath)
        {
            HashSet<string> Females = new HashSet<string>();
            HashSet<string> Males = new HashSet<string>();
            try
            {
                
                foreach (string line in File.ReadLines(filePath))
                {  
                    Console.WriteLine(line);
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

                Console.WriteLine("\nFemales: " + String.Join(", ", Females));
                Console.WriteLine("Males: " + String.Join(", ", Males));

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

    class Program
    {
        static void Main(string[] args)
        {
            // const string MidStr = "matches";

            Utility Ut = new Utility();

            // string Name1 = "mona";
            // string Name2 = "lisa";

            // // Test CreateMatchString
            // Console.WriteLine(Ut.CreateMatchString(Name1, Name2, MidStr));

            // // Test CountMatchChars
            // List<int> CountsList = Ut.CountChars(Name1, Name2, MidStr);
            // Console.WriteLine("CountsList: " + String.Join(", ", CountsList));

            // // Test CountDigits
            // Console.WriteLine(Ut.CountDigits(-100000000)); // expect 9

            // // Test ReduceDigits
            // List<int> Counts = new List<int> {2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2};
            // Console.WriteLine(Ut.ReduceDigits(Counts)); // expect: 60

            // // Test IsAlphabetic
            // string Str = "@abdc";
            // Console.WriteLine(Ut.IsAlphabetic(Str));

            // Test
            const string IOFolder = "io_folder/";
            Ut.ReadCSV(IOFolder + "names.csv");
        }
    }
}
