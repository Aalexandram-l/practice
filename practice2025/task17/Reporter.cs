using System;
using System.IO;

namespace task17
{
    public static class Reporter
    {
        private const string Txt = "Report.txt";
        private const string Csv = "report.csv";

        public static void Init()
        {
            File.Delete(Txt);
            File.WriteAllText(Csv, "time,id,counter\n");
        }

        public static void Write(string text)
        {
            File.AppendAllText(Txt, $"{DateTime.Now:HH:mm:ss.fff}  {text}\n");
        }

        public static void WriteCsv(int id, int counter)
        {
            File.AppendAllText(Csv, $"{DateTime.Now:HH:mm:ss.fff},{id},{counter}\n");
        }
    }
}