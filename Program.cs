using System;
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

    class Match : IComparable
    {
        public static string MidStr { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Score { get; set; }
        public int MinGoodScore { get; set; }

        public Match(string name1, string name2, int score=0)
        {
            Name1 = name1;
            Name2 = name2;
            Score = score;
            MidStr = "matches";
            MinGoodScore = 80;
        }

        public string GetMatchString()
        {
            return Name1 + MidStr + Name2;
        }

        public string GetFinalMatchString()
        {
            string Result = String.Format(
                "{0} {1} {2} {3}%",
                Name1, MidStr, Name2, Score
            );

            if (Score >= MinGoodScore)
            {
                Result += ", good match";
            }

            return Result;
        }

        public override string ToString() => this.GetFinalMatchString();
    
        // public override string ToString()
        // {
        //     return String.Format(
        //         "({0}, {1}, {2})", 
        //         Name1, Name2, Score
        //     );
        // }
    
        int IComparable.CompareTo(object obj)
        {
            Match Other = (Match) obj;
            if (this.Score == Other.Score)
            {
                return String.Compare(
                    this.GetMatchString(), Other.GetMatchString()
                );
            }
            return (this.Score < Other.Score) ? 1: -1;
        }
    }

    class Utility<T>
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
        
        public List<Match> MatchUp(HashSet<string> nameList1, HashSet<string> nameList2)
        {
            List<Match> Matches = new List<Match>();

            foreach (string name1 in nameList1)
            {
                foreach (string name2 in nameList2)
                {
                    Matches.Add(new Match(name1, name2));
                }
            }

            return Matches;
        }
    
        public void SaveListToFile(string filename, List<T> itemList)
        {
            using(TextWriter tw = new StreamWriter(filename))
            {
                foreach (T item in itemList)
                {
                    tw.WriteLine(item.ToString());
                }
            }
        }
    }

    class Test
    {
        public void RunLocalTests()
        {
            const string MidStr = "matches";

            Utility<Match> Ut = new Utility<Match>();

            string Name1 = "mona";
            string Name2 = "lisa";

            // Test CreateMatchString
            Console.WriteLine("Test CreateMatchString:");
            Console.WriteLine(Ut.CreateMatchString(Name1, Name2, MidStr));
            Console.WriteLine();

            // Test CountMatchChars
            Console.WriteLine("Test CountMatchChars:");
            List<int> CountsList = Ut.CountChars(Name1, Name2, MidStr);
            Console.WriteLine("CountsList: " + String.Join(", ", CountsList));
            Console.WriteLine();

            // Test CountDigits
            Console.WriteLine("Test CountDigits:");
            Console.WriteLine(Ut.CountDigits(-100000000)); // expect 9
            Console.WriteLine();

            // Test ReduceDigits
            Console.WriteLine("Test ReduceDigits:");
            List<int> Counts = new List<int> {2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2};
            Console.WriteLine(Ut.ReduceDigits(Counts)); // expect: 60
            Console.WriteLine();

            // Test IsAlphabetic
            Console.WriteLine("Test IsAlphabetic:");
            string Str = "@abdc";
            Console.WriteLine(Ut.IsAlphabetic(Str));
            Console.WriteLine();

            // Test ReadCSV
            Console.WriteLine("Test ReadCSV:");
            const string IOFolder = "io_folder/";
            List<HashSet<string>> AllNames = Ut.ReadCSV(IOFolder + "names.csv");
            Console.WriteLine("Females: " + String.Join(", ", AllNames[0]));
            Console.WriteLine("Males: " + String.Join(", ", AllNames[1]));
            Console.WriteLine();

            // Test MatchUp
            Console.WriteLine("Test MatchUp:");
            List<Match> Matches = Ut.MatchUp(AllNames[0], AllNames[1]);
            Console.WriteLine("Matches: " + String.Join(", ", Matches));
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string MidStr = "matches";
            const int MinGoodScore = 80;     // Minimum good match score

            Utility<Match> Ut = new Utility<Match>();

            // // Run local tests
            // Test TestC = new Test();
            // TestC.RunLocalTests();

            const string IOFolder = "io_folder/";
            const string InpFileName = IOFolder + "names_1.csv";
            const string OutFileName = IOFolder + "output_1.txt";

            // Read Females and Males sets from csv file
            List<HashSet<string>> AllNames = Ut.ReadCSV(InpFileName);

            // Match-up the Females and Males sets
            List<Match> Matches = Ut.MatchUp(AllNames[0], AllNames[1]);

            // Calculate and store each match's score
            foreach (Match MatchObj in Matches)
            {
                List<int> Counts = Ut.CountChars(
                    MatchObj.Name1, MatchObj.Name2, MidStr
                );
                MatchObj.Score = Ut.ReduceDigits(Counts);
            }
            // Matches.Add(new Match("ace", "billy", 95));
            Console.WriteLine("Matches: " + String.Join(", ", Matches));
            
            // Sort Matches
            Matches.Sort();
            Console.WriteLine("Matches: " + String.Join(", ", Matches));

            // Save Matches to text file
            Ut.SaveListToFile(OutFileName, Matches);
        }
    }
}
