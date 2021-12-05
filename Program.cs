using System;
using System.Collections.Generic;

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
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string MidStr = "matches";

            Utility Ut = new Utility();

            string Name1 = "mona";
            string Name2 = "lisa";

            // Test CreateMatchString
            Console.WriteLine(Ut.CreateMatchString(Name1, Name2, MidStr));

            // Test CountMatchChars
            List<int> CountsList = Ut.CountChars(Name1, Name2, MidStr);
            Console.WriteLine("CountsList: " + String.Join(", ", CountsList));
        }
    }
}
