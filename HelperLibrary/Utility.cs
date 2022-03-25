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
            List<int> countsList = new List<int>();

            Dictionary<char, int> charsDict = 
                new Dictionary<char, int>();
            
            foreach (char c in str)
            {
                if (charsDict.ContainsKey(c))
                {
                    charsDict[c] += 1;
                }
                else
                {
                    charsDict.Add(c, 1);
                }
            }

            HashSet<char> seenChars = new HashSet<char>();
            foreach (char c in str)
            {
                if (!seenChars.Contains(c))
                {
                    countsList.Add(charsDict[c]);
                    seenChars.Add(c);
                }
            }

            return countsList;
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
                List<int> newCounts = new List<int>();
                while (i <= j)
                {
                    int num;
                    // Add the leftmost and righmost numbers
                    if (i == j)
                    {
                        num = counts[i];
                    }
                    else
                    {
                        num = counts[i] + counts[j];
                    }

                    // Check if Num is a single- or 2-digit number
                    if (CountDigits(num) > 1)
                    {
                        newCounts.Add((int) (num / 10));
                        newCounts.Add(num % 10);
                    }
                    else
                    {
                        newCounts.Add(num);
                    }

                    i++;
                    j--;
                }

                counts = newCounts;
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
            List<string[]> products = new List<string[]>();

            foreach (string name1 in nameList1)
            {
                foreach (string name2 in nameList2)
                {
                    products.Add(new string[] { name1, name2 });
                }
            }

            return products;
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
                string message = "Failed to write output file:" + e.Message;
                LoggerObj.WriteLineToLog(
                    message, LogType.Fatal
                );
                Console.WriteLine(message);
                Console.WriteLine("\nProgram terminating.");
                Environment.Exit(0);
            }
        }
    }
}
