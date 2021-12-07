using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace HelperLibrary
{
    public class Utility
    {
        public static List<int> CountChars(string str)
        {
            List<int> CountsList = new List<int>();

            Dictionary<char, int> CharsDict = 
                new Dictionary<char, int>();
            
            foreach (char c in str)
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
            foreach (char c in str)
            {
                if (!SeenChars.Contains(c))
                {
                    CountsList.Add(CharsDict[c]);
                    SeenChars.Add(c);
                }
            }

            return CountsList;
        }

        public static int CountDigits(int number)
        {
            return number == 0 ? 1 : (int) Math.Floor(Math.Log10(Math.Abs(number)) + 1);
        }

        public static int ReduceDigits(List<int> counts)
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

        public static bool IsAlphabetic(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");

        }
        
        public static List<string[]> CrossStringLists(HashSet<string> nameList1, HashSet<string> nameList2)
        {
            List<string[]> Products = new List<string[]>();

            foreach (string name1 in nameList1)
            {
                foreach (string name2 in nameList2)
                {
                    Products.Add(new string[] { name1, name2 });
                }
            }

            return Products;
        }
    
        public static void SaveListToFile(string filename, List<string> itemList, Logger LoggerObj)
        {
            try
            {
                using(TextWriter tw = new StreamWriter(filename))
                {
                    foreach (string item in itemList)
                    {
                        tw.WriteLine(item.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                string Message = "Failed to write output file:" + e.Message;
                LoggerObj.WriteLineToLog(
                    Message, LogType.Fatal
                );
                Console.WriteLine(Message);
                Console.WriteLine("\nProgram terminating.");
                Environment.Exit(0);
            }
        }
    }
}
