using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TennisMatch
{
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

                    i += 1;
                    j -=1;
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string MidStr = "matches";

            Utility Ut = new Utility();

            string Name1 = "mona";
            string Name2 = "lisa";

            // // Test CreateMatchString
            // Console.WriteLine(Ut.CreateMatchString(Name1, Name2, MidStr));

            // // Test CountMatchChars
            // List<int> CountsList = Ut.CountChars(Name1, Name2, MidStr);
            // Console.WriteLine("CountsList: " + String.Join(", ", CountsList));

            // Test CountDigits
            Console.WriteLine(Ut.CountDigits(-100000000)); // expect 9

            // Test ReduceDigits
            List<int> Counts = new List<int> {2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2};
            Console.WriteLine(Ut.ReduceDigits(Counts)); // expect: 60

            // Test IsAlphabetic
            string Str = "@abdc";
            Console.WriteLine(Ut.IsAlphabetic(Str));
        }
    }
}
