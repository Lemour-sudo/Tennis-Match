using System;
using System.IO;

namespace HelperLibrary
{
    public static class LogType
    {
        public const string Info = "INFO   ";
        public const string Warning = "WARNING";
        public const string Fatal = "FATAL  ";
    }

    public class Logger
    {
        public string PathFolder { get; }
        public string FullPath { get; set; }

        public Logger(string pathFolder)
        {
            PathFolder = pathFolder;

            // Cook-up full file-path
            CreateFullPath(pathFolder);

            // Create log file
            this.CreateFile();
        }

        private void CreateFullPath(string pathFolder)
        {
            DateTime now = DateTime.Now;
            string indentifier = String.Format(
                "applog_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}",
                now.Year, now.Month, now.Day, 
                now.Hour, now.Minute, now.Second
            );
            FullPath = pathFolder + indentifier + ".log";
        }

        private void CreateFile()
        {
            Directory.CreateDirectory(PathFolder);

            try
            {
                using (TextWriter tw = new StreamWriter(File.Open(FullPath, FileMode.CreateNew)))
                {
                    DateTime now = DateTime.Now;
                    string timeStamp = String.Format(
                        "{0:00}:{1:00}:{2:00}",
                        now.Hour, now.Minute, now.Second
                    );
                    tw.WriteLine("You may find these logs helpful ;)");
                    tw.WriteLine("==================================");
                    tw.WriteLine();
                    tw.WriteLine(String.Format(
                        "{0}  [{1}]  Program started.", 
                        timeStamp, LogType.Info
                    ));
                    tw.Close();
                    tw.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "\nLog file creation failed: " + e.Message +
                    "\nProgram will not be logging during execution.\n"
                );
            }

        }

        public void WriteLineToLog(string message, string type)
        {
            if (!File.Exists(FullPath))
            {
                return;
            }

            DateTime now = DateTime.Now;
            string timeStamp = String.Format(
                "{0:00}:{1:00}:{2:00}",
                now.Hour, now.Minute, now.Second
            );

            string logMsg = String.Format(
                "{0}  [{1}]  {2}",
                timeStamp, type, message
            );

            try
            {
                using(TextWriter tw = new StreamWriter(File.Open(FullPath, FileMode.Append)))
                {
                    tw.WriteLine(logMsg);
                    
                    if (type == "FATAL")
                    {
                        tw.WriteLine(String.Format(
                            "{0}  [{1}]  Program terminated due to Fatal Error.", 
                            timeStamp, LogType.Fatal
                        ));
                    }

                    tw.Close();
                    tw.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "\nLog could not be recorded: " + e.Message + "\n"
                );
            }
        }
    }
}
