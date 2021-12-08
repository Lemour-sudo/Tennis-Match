using System;

namespace HelperLibrary
{
    public class Match : IComparable
    {
        public static string MidStr = "matches"; // used for connecting two names
        public static int MinGoodScore = 60;
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int Score { get; set; }

        public Match(string name1, string name2, int score=0)
        {
            Name1 = name1.ToLower().Trim();
            Name2 = name2.ToLower().Trim();
            Score = score;
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

        public override string ToString()
        {
            return this.GetFinalMatchString();
        }         
    
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
}
